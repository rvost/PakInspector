meta:
  id: pak
  file-extension: pak

seq:
  - id: form
    size: 4
    contents: FORM
  - id: length
    type: u4be
  - id: form_type
    size: 4
    contents: PAC1
  - id: chunks
    type: chunk
    repeat: eos

types:
  chunk:
    seq:
      - id: type_id
        type: u4le # use int32 representation for efficient comparison in the switch
        enum: chunk_type
      - id: length
        type: u4be
      - id: body
        size: length
        type:
          switch-on: type_id
          cases:
            'chunk_type::head': head_chunk
            'chunk_type::data': data_chunk
            'chunk_type::file': file_chunk
            _ : raw_chunk
    enums:
      chunk_type: 
        0x44414548: head # HEAD
        0x41544144: data # DATA
        0x454C4946: file # FILE
  head_chunk:
    seq:
      - id: header # probably contains checksum/signature
        size-eos: true
  data_chunk:
    seq:
      - id: content
        size-eos: true
  file_chunk:
    seq:
      - id: root
        type: pak_entry
  pak_entry:
    seq:
      - id: entry_type
        type: u1
      - id: name_length
        type: u1
      - id: name
        type: str
        size: name_length
        encoding: UTF-8
      - id: info
        type:
          switch-on: entry_type
          cases:
            0: pak_folder_info
            1: pak_file_info
    types:
      pak_folder_info:
        seq:
          - id: child_count
            type: u4le
          - id: children
            type: pak_entry
            repeat: expr
            repeat-expr: child_count
      pak_file_info:
        seq:
          - id: offset
            type: u4le
          - id: compressed_length
            type: u4le
          - id: original_length
            type: u4le
          - id: unknown1 # usually 0 probably padding
            size: 4
          - id: compression_type
            type: u4be
          - id: unknown2
            size: 4
        instances:
          data:
            io: _root._io
            pos: offset
            size: compressed_length
  raw_chunk: #distinguish from known chunks
    seq:
      - id: unknown
        size-eos: true