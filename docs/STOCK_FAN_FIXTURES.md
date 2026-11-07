# Reviewed stock firmware packets

`stock_fixtures.rs` contains exactly the first 90 bytes of captured PlatformConfigurationAccessV1 replies. They are numeric data, not executable firmware. The stock application path still retrieves live packets and requires byte-for-byte equality before writing.

| Preset | Input bytes | SHA-256 of 90 bytes |
| --- | --- | --- |
| Balanced | 2, 2, 1, 0 | a01d000d5e5d02578fc1dbad362c454a6172edd1edc854a1d3ddd5bee9361bdf |
| Cool | 2, 2, 2, 0 | f1d78e7155a4e89d700fa53f26457b67c3fa11d69caee78d2ac4cbaef0c278a6 |

The existing 94-byte test036 fixture supplies reviewed preserved-gap bytes. Projection follows the inspected driver mapping, skipping two EC bytes after each 14-byte record. The last three packet bytes target fields outside the readable span; their zero values are stock packet content, not evidence of original live values.
