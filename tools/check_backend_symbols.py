"""Read-only PE CodeView / PDB identity check for a selected backend build."""
from pathlib import Path
import hashlib
import json
import struct
import sys
import uuid

exe, pdb, output = map(Path, sys.argv[1:])
data = exe.read_bytes()
pe = struct.unpack_from('<I', data, 0x3c)[0]
optional = pe + 24
assert data[pe:pe + 4] == b'PE\0\0'
assert struct.unpack_from('<H', data, optional)[0] == 0x20b
sections = struct.unpack_from('<H', data, pe + 6)[0]
section_start = optional + struct.unpack_from('<H', data, pe + 20)[0]

def raw(rva):
    for i in range(sections):
        offset = section_start + i * 40
        size, address, raw_size, raw_address = struct.unpack_from('<IIII', data, offset + 8)
        if address <= rva < address + max(size, raw_size):
            return raw_address + rva - address
    raise ValueError('Unmapped PE RVA')

debug_rva, debug_size = struct.unpack_from('<II', data, optional + 112 + 6 * 8)
assert debug_rva and debug_size
codeview = None
for offset in range(raw(debug_rva), raw(debug_rva) + debug_size, 28):
    _, _, _, _, kind, size, _, pointer = struct.unpack_from('<IIHHIIII', data, offset)
    if kind == 2 and data[pointer:pointer + 4] == b'RSDS':
        codeview = {'guid': str(uuid.UUID(bytes_le=data[pointer + 4:pointer + 20])),
                    'age': struct.unpack_from('<I', data, pointer + 20)[0]}
assert codeview is not None
blob = pdb.read_bytes()
assert blob.startswith(b'Microsoft C/C++ MSF 7.00\r\n\x1aDS\0\0\0')
block_size, _, block_count, directory_size, _, block_map = struct.unpack_from('<IIIIII', blob, 32)
assert block_count * block_size == len(blob)
directory_blocks = struct.unpack_from('<' + 'I' * ((directory_size + block_size - 1) // block_size), blob, block_map * block_size)
directory = b''.join(blob[b * block_size:(b + 1) * block_size] for b in directory_blocks)[:directory_size]
count = struct.unpack_from('<I', directory)[0]
sizes = struct.unpack_from('<' + 'I' * count, directory, 4)
offset = 4 + count * 4
info = None
for index, size in enumerate(sizes):
    if size == 0xffffffff:
        continue
    blocks_needed = (size + block_size - 1) // block_size
    blocks = struct.unpack_from('<' + 'I' * blocks_needed, directory, offset) if blocks_needed else ()
    offset += 4 * blocks_needed
    if index == 1:
        info = b''.join(blob[b * block_size:(b + 1) * block_size] for b in blocks)[:size]
assert info is not None
pdb_identity = {'guid': str(uuid.UUID(bytes_le=info[12:28])), 'age': struct.unpack_from('<I', info, 8)[0]}
assert codeview == pdb_identity, 'PDB does not match executable'
result = {'executable_sha256': hashlib.sha256(data).hexdigest(),
          'pdb_sha256': hashlib.sha256(blob).hexdigest(), 'codeview': codeview,
          'pdb_identity': pdb_identity, 'pdb_matches_executable': True,
          'scope': 'Static PE/PDB identity only; no execution'}
assert not output.exists(), 'Preserve prior identity evidence'
output.parent.mkdir(parents=True, exist_ok=True)
output.write_text(json.dumps(result, indent=2) + '\n')
print(json.dumps(result))
