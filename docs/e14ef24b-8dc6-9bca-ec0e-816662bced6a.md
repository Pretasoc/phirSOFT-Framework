# ArrayTools.GetArrayPart(*T*) Method 
 

Reads a part of an generic <a href="http://msdn2.microsoft.com/en-us/library/czz5hkty" target="_blank">Array</a> and returns it.

**Namespace:**&nbsp;<a href="e822f0a1-f524-76ce-c72d-9a62b8c4e673">phirSOFT.Common</a><br />**Assembly:**&nbsp;phirSOFT.Common (in phirSOFT.Common.dll) Version: 2.0.0.0

## Syntax

**C#**<br />
``` C#
public static T[] GetArrayPart<T>(
	this T[] array,
	long start,
	long elementCount
)

```

**VB**<br />
``` VB
<ExtensionAttribute>
Public Shared Function GetArrayPart(Of T) ( 
	array As T(),
	start As Long,
	elementCount As Long
) As T()
```

**VB Usage**<br />
``` VB Usage
Dim array As T()
Dim start As Long
Dim elementCount As Long
Dim returnValue As T()

returnValue = array.GetArrayPart(start, 
	elementCount)
```


#### Parameters
&nbsp;<dl><dt>array</dt><dd>Type: *T*[]<br />The <a href="http://msdn2.microsoft.com/en-us/library/czz5hkty" target="_blank">Array</a> to extract the subarray from.</dd><dt>start</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/6yy583ek" target="_blank">System.Int64</a><br />The zero based index to start extracting data.</dd><dt>elementCount</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/6yy583ek" target="_blank">System.Int64</a><br />The amount of elements in the target <a href="http://msdn2.microsoft.com/en-us/library/czz5hkty" target="_blank">Array</a>.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>\[Missing <typeparam name="T"/> documentation for "M:phirSOFT.Common.ArrayTools.GetArrayPart``1(``0[],System.Int64,System.Int64)"\]</dd></dl>

#### Return Value
Type: *T*[]<br />An array containing the subarray.

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type . When you use instance method syntax to call this method, omit the first parameter. For more information, see <a href="http://msdn.microsoft.com/en-us/library/bb384936.aspx">Extension Methods (Visual Basic)</a> or <a href="http://msdn.microsoft.com/en-us/library/bb383977.aspx">Extension Methods (C# Programming Guide)</a>.

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/8xt94y6e" target="_blank">ArgumentOutOfRangeException</a></td><td>Thrown, if *start* is not in [0 - *array.Lenght.Lenght*], or *elementCount* is not in [1 - (*array.Lenght.Lenght* - *start* )].</td></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/27426hcy" target="_blank">ArgumentNullException</a></td><td>Thrown, if *array* is a null reference (`Nothing` in Visual Basic).</td></tr></table>

## See Also


#### Reference
<a href="57569303-b3dd-8201-fb50-fabefa82e02a">ArrayTools Class</a><br /><a href="e822f0a1-f524-76ce-c72d-9a62b8c4e673">phirSOFT.Common Namespace</a><br />