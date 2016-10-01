# InvalidSizeException Constructor (SerializationInfo, StreamingContext)
 

Initializes a new instance of the <a href="e2f2c151-0226-3f11-1ac9-0d2c03ac0c3c">InvalidSizeException</a> class

**Namespace:**&nbsp;<a href="e822f0a1-f524-76ce-c72d-9a62b8c4e673">phirSOFT.Common</a><br />**Assembly:**&nbsp;phirSOFT.Common (in phirSOFT.Common.dll) Version: 2.0.0.0

## Syntax

**C#**<br />
``` C#
protected InvalidSizeException(
	SerializationInfo info,
	StreamingContext context
)
```

**VB**<br />
``` VB
Protected Sub New ( 
	info As SerializationInfo,
	context As StreamingContext
)
```

**VB Usage**<br />
``` VB Usage
Dim info As SerializationInfo
Dim context As StreamingContext

Dim instance As New InvalidSizeException(info, context)
```


#### Parameters
&nbsp;<dl><dt>info</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/a9b6042e" target="_blank">System.Runtime.Serialization.SerializationInfo</a><br />\[Missing <param name="info"/> documentation for "M:phirSOFT.Common.InvalidSizeException.#ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)"\]</dd><dt>context</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/t16abws5" target="_blank">System.Runtime.Serialization.StreamingContext</a><br />\[Missing <param name="context"/> documentation for "M:phirSOFT.Common.InvalidSizeException.#ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)"\]</dd></dl>

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/27426hcy" target="_blank">ArgumentNullException</a></td><td>*info* is a null reference (`Nothing` in Visual Basic).</td></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/akw26cdk" target="_blank">SerializationException</a></td><td>The deserialization of the exception failed. See Inner Exception for details.</td></tr></table>

## See Also


#### Reference
<a href="e2f2c151-0226-3f11-1ac9-0d2c03ac0c3c">InvalidSizeException Class</a><br /><a href="76e5ba0e-b0fc-f470-0c06-3b55d1ece1f3">InvalidSizeException Overload</a><br /><a href="e822f0a1-f524-76ce-c72d-9a62b8c4e673">phirSOFT.Common Namespace</a><br />