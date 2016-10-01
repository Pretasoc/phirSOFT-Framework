# HashSum(*T*).FromObject Method (Byte[], Int32, Int32)
 

\[Missing <summary> documentation for "M:phirSOFT.Common.HashSum`1.FromObject(System.Byte[],System.Int32,System.Int32)"\]

**Namespace:**&nbsp;<a href="e822f0a1-f524-76ce-c72d-9a62b8c4e673">phirSOFT.Common</a><br />**Assembly:**&nbsp;phirSOFT.Common (in phirSOFT.Common.dll) Version: 2.0.0.0

## Syntax

**C#**<br />
``` C#
public static HashSum<T> FromObject(
	byte[] data,
	int offset,
	int count
)
```

**VB**<br />
``` VB
Public Shared Function FromObject ( 
	data As Byte(),
	offset As Integer,
	count As Integer
) As HashSum(Of T)
```

**VB Usage**<br />
``` VB Usage
Dim data As Byte()
Dim offset As Integer
Dim count As Integer
Dim returnValue As HashSum(Of T)

returnValue = HashSum.FromObject(data, 
	offset, count)
```


#### Parameters
&nbsp;<dl><dt>data</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/yyb1w04y" target="_blank">System.Byte</a>[]<br /></dd><dt>offset</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br /></dd><dt>count</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br /></dd></dl>

#### Return Value
Type: <a href="2ba12663-0b38-f3a5-8601-53777204340c">HashSum</a>(<a href="2ba12663-0b38-f3a5-8601-53777204340c">*T*</a>)<br />\[Missing <returns> documentation for "M:phirSOFT.Common.HashSum`1.FromObject(System.Byte[],System.Int32,System.Int32)"\]

## See Also


#### Reference
<a href="2ba12663-0b38-f3a5-8601-53777204340c">HashSum(T) Structure</a><br /><a href="c2fc0e43-c923-ab62-d86a-2ab2d035c8d4">FromObject Overload</a><br /><a href="e822f0a1-f524-76ce-c72d-9a62b8c4e673">phirSOFT.Common Namespace</a><br />