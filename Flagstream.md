phirSOFT Specification (Flagstream)
===================================

*Version: 1.0*


The flagstream is a stream, where the first bit of each byte determinates, wheter the stream end or continues.

The other bits can represent any other stream. Is is designet to use in binary file formats, which stores a lot of boolean values.

### Implementation
    bool Continue(byte data){
		return (data & (1 << 7));
	}