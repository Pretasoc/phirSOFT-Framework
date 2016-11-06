using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using JetBrains.Annotations;

namespace phirSOFT.Common.Controls.Tasks
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
    [PublicAPI]
    public class ToolStripTaskHost : ToolStripItem
    {
        private readonly ICollection<InternalProgressInfo> _progresses;
        private readonly BoolValueConcurrencyProxy _visible;
        private double _progress;
        private readonly object _progressCollectionLock = new object();

        public ToolStripTaskHost()
        {
            _visible = new BoolValueConcurrencyProxy(true);
            base.Visible = _visible.Value;
            _visible.ValueChanged += (sender, e) => base.Visible = _visible.Value;

            _progresses = new ProgressCollection(this);
        }

        private Rectangle LabelRectangle => new Rectangle(102, 0, ContentRectangle.Width - 102, ContentRectangle.Height)
            ;

        protected override Size DefaultSize => GetPreferredSize(new Size());


        /// <inheritdoc />
        public override bool CanSelect => DesignMode;

        /// <summary>
        /// Gets the overall progress of all tasks.
        /// </summary>
        public double Progress
        {
            get { return _progress; }
            private set
            {
                _progress = value;
                Invalidate();
            }
        }

        public new bool Visible
        {
            get { return _visible.User; }

            set { _visible.User = value; }
        }

        protected override void OnOwnerChanged(EventArgs e)
        {
            _visible.Application = DesignMode;
            base.OnOwnerChanged(e);
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            if (Owner == null) return base.GetPreferredSize(constrainingSize);
            var size = TextRenderer.MeasureText("100,00 %", Font);
            size.Width += 102;
            return size;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            Size = GetPreferredSize(new Size());
            base.OnFontChanged(e);
        }


        public IProgress<ProgressInfo> RegisterProgress()
        {
            var progress = new InternalProgressInfo();
            progress.ProgressChanged += Progress_ProgressChanged;
            lock (_progressCollectionLock)
            {
                _progresses.Add(progress);
            }
            return progress;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Owner == null) return;
            var renderer = Owner.Renderer;

            renderer.DrawLabelBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
            ProgressBarRenderer.DrawHorizontalBar(e.Graphics, new Rectangle(1, (Height - 16)/2 - 1, 100, 16));
            ProgressBarRenderer.DrawHorizontalChunks(e.Graphics,
                new Rectangle(1, (Height - 16)/2 - 1, (int) (100*Progress), 16));
            renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(e.Graphics,
                this,
                $"{Progress:P}",
                LabelRectangle,
                ForeColor,
                Font,
                ContentAlignment.MiddleLeft));
        }

        private void Progress_ProgressChanged(object sender, ProgressInfo e)
        {
            var progress = sender as InternalProgressInfo;

            if (progress == null) return;

            // Here we update only the overall progress.
            lock (_progressCollectionLock)
            {
                Progress = _progresses.Sum(p => p.Info.PercentComplete)/_progresses.Count;
            }
        }


        public IProgress<ProgressInfo> RegisterProgress(Task task)
        {
            var progress = new InternalProgressInfo {Task = task};
            progress.ProgressChanged += Progress_ProgressChanged;
            lock (_progressCollectionLock)
            {
                _progresses.Add(progress);
            }
            return progress;
        }

        public void DoneProgress(IProgress<ProgressInfo> info)
        {
            var progressInfo = info as InternalProgressInfo;
            if (progressInfo == null) return;
            lock (_progressCollectionLock) {
                _progresses.Remove(progressInfo);
            }
        }


        private class InternalProgressInfo : Progress<ProgressInfo>
        {
            public Task Task { get; set; }

            public ProgressInfo Info { get; private set; } = new ProgressInfo();

            protected override void OnReport(ProgressInfo value)
            {
                Info = value;
                base.OnReport(value);
            }
        }

        private class ProgressCollection : Collection<InternalProgressInfo>
        {
            private readonly ToolStripTaskHost _owner;

            public ProgressCollection(ToolStripTaskHost owner)
            {
                _owner = owner;
            }

            protected override void InsertItem(int index, InternalProgressInfo item)
            {
                base.InsertItem(index, item);
                if (Count > 0)
                    _owner._visible.Application = true;

                _owner.ToolTipText = $"{Count} Tasks pending.";
            }

            protected override void RemoveItem(int index)
            {
                base.RemoveItem(index);
                if (Count == 0)
                    _owner._visible.Application = _owner.DesignMode;

                _owner.ToolTipText = $"{Count} Tasks pending.";
            }

            protected override void ClearItems()
            {
                base.ClearItems();
                if (Count == 0)
                    _owner._visible.Application = _owner.DesignMode;

                _owner.ToolTipText = $"{Count} Tasks pending.";
            }
        }
    }
}