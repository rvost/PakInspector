meta:
  id: iff
  title: Interchange File Format
  justsolve: IFF

seq:
  - id: form
    size: 4
    contents: FORM
  - id: length
    type: u4be
  - id: form_type
    # Use str for convenience,
    # but note that form_type can contain non-ASCII symbols.
    type: str 
    size: 4
    encoding: ASCII
  - id: chunks
    type: chunk
    repeat: eos

types:
  chunk:
    seq:
      - id: type_id
        type: str # same as form_type
        size: 4
        encoding: ASCII
      - id: length
        type: u4be
      - id: body
        size: length