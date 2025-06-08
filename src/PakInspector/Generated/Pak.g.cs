using Kaitai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

namespace PakInspector;

public partial class Pak : KaitaiStruct
{
    public static Pak FromFile(string fileName)
    {
        return new Pak(new KaitaiStream(fileName));
    }

    public Pak(KaitaiStream p__io, KaitaiStruct p__parent = null, Pak p__root = null) : base(p__io)
    {
        m_parent = p__parent;
        m_root = p__root ?? this;
        _read();
    }
    private void _read()
    {
        _form = m_io.ReadBytes(4);
        if (!((KaitaiStream.ByteArrayCompare(Form, new byte[] { 70, 79, 82, 77 }) == 0)))
        {
            throw new ValidationNotEqualError(new byte[] { 70, 79, 82, 77 }, Form, M_Io, "/seq/0");
        }
        _length = m_io.ReadU4be();
        _formType = m_io.ReadBytes(4);
        if (!((KaitaiStream.ByteArrayCompare(FormType, new byte[] { 80, 65, 67, 49 }) == 0)))
        {
            throw new ValidationNotEqualError(new byte[] { 80, 65, 67, 49 }, FormType, M_Io, "/seq/2");
        }
        _chunks = new List<Chunk>();
        {
            var i = 0;
            while (!m_io.IsEof)
            {
                _chunks.Add(new Chunk(m_io, this, m_root));
                i++;
            }
        }
    }
    public partial class Chunk : KaitaiStruct
    {
        public static Chunk FromFile(string fileName)
        {
            return new Chunk(new KaitaiStream(fileName));
        }


        public enum ChunkType
        {
            Data = 1096040772,
            Head = 1145128264,
            File = 1162627398,
        }
        public Chunk(KaitaiStream p__io, Pak p__parent = null, Pak p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _typeId = ((ChunkType)m_io.ReadU4le());
            _length = m_io.ReadU4be();
            switch (TypeId)
            {
                case ChunkType.Head:
                    {
                        __raw_body = m_io.ReadBytes(Length);
                        var io___raw_body = new KaitaiStream(__raw_body);
                        _body = new HeadChunk(io___raw_body, this, m_root);
                        break;
                    }
                case ChunkType.Data:
                    {
                        __raw_body = m_io.ReadBytes(Length);
                        var io___raw_body = new KaitaiStream(__raw_body);
                        _body = new DataChunk(io___raw_body, this, m_root);
                        break;
                    }
                case ChunkType.File:
                    {
                        __raw_body = m_io.ReadBytes(Length);
                        var io___raw_body = new KaitaiStream(__raw_body);
                        _body = new FileChunk(io___raw_body, this, m_root);
                        break;
                    }
                default:
                    {
                        __raw_body = m_io.ReadBytes(Length);
                        var io___raw_body = new KaitaiStream(__raw_body);
                        _body = new RawChunk(io___raw_body, this, m_root);
                        break;
                    }
            }
        }
        private ChunkType _typeId;
        private uint _length;
        private KaitaiStruct _body;
        private Pak m_root;
        private Pak m_parent;
        private byte[] __raw_body;
        public ChunkType TypeId { get { return _typeId; } }
        public uint Length { get { return _length; } }
        public KaitaiStruct Body { get { return _body; } }
        public Pak M_Root { get { return m_root; } }
        public Pak M_Parent { get { return m_parent; } }
        public byte[] M_RawBody { get { return __raw_body; } }
    }
    public partial class HeadChunk : KaitaiStruct
    {
        public static HeadChunk FromFile(string fileName)
        {
            return new HeadChunk(new KaitaiStream(fileName));
        }

        public HeadChunk(KaitaiStream p__io, Pak.Chunk p__parent = null, Pak p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _header = m_io.ReadBytesFull();
        }
        private byte[] _header;
        private Pak m_root;
        private Pak.Chunk m_parent;
        public byte[] Header { get { return _header; } }
        public Pak M_Root { get { return m_root; } }
        public Pak.Chunk M_Parent { get { return m_parent; } }
    }
    public partial class PakEntry : KaitaiStruct
    {
        public static PakEntry FromFile(string fileName)
        {
            return new PakEntry(new KaitaiStream(fileName));
        }

        public PakEntry(KaitaiStream p__io, KaitaiStruct p__parent = null, Pak p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _entryType = m_io.ReadU1();
            _nameLength = m_io.ReadU1();
            _name = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytes(NameLength));
            switch (EntryType)
            {
                case 0:
                    {
                        _info = new PakFolderInfo(m_io, this, m_root);
                        break;
                    }
                case 1:
                    {
                        _info = new PakFileInfo(m_io, this, m_root);
                        break;
                    }
            }
        }
        public partial class PakFolderInfo : KaitaiStruct
        {
            public static PakFolderInfo FromFile(string fileName)
            {
                return new PakFolderInfo(new KaitaiStream(fileName));
            }

            public PakFolderInfo(KaitaiStream p__io, Pak.PakEntry p__parent = null, Pak p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _childCount = m_io.ReadU4le();
                _children = new List<PakEntry>();
                for (var i = 0; i < ChildCount; i++)
                {
                    _children.Add(new PakEntry(m_io, this, m_root));
                }
            }
            private uint _childCount;
            private List<PakEntry> _children;
            private Pak m_root;
            private Pak.PakEntry m_parent;
            public uint ChildCount { get { return _childCount; } }
            public List<PakEntry> Children { get { return _children; } }
            public Pak M_Root { get { return m_root; } }
            public Pak.PakEntry M_Parent { get { return m_parent; } }
        }
        public partial class PakFileInfo : KaitaiStruct
        {
            public static PakFileInfo FromFile(string fileName)
            {
                return new PakFileInfo(new KaitaiStream(fileName));
            }

            public PakFileInfo(KaitaiStream p__io, Pak.PakEntry p__parent = null, Pak p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_data = false;
                _read();
            }
            private void _read()
            {
                _offset = m_io.ReadU4le();
                _compressedLength = m_io.ReadU4le();
                _originalLength = m_io.ReadU4le();
                _unknown1 = m_io.ReadBytes(4);
                _compressionType = m_io.ReadU4be();
                _unknown2 = m_io.ReadBytes(4);
            }
            private bool f_data;
            private byte[] _data;
            public byte[] Data
            {
                get
                {
                    if (f_data)
                        return _data;
                    KaitaiStream io = M_Root.M_Io;
                    long _pos = io.Pos;
                    io.Seek(Offset);
                    _data = io.ReadBytes(CompressedLength);
                    io.Seek(_pos);
                    f_data = true;
                    return _data;
                }
            }
            private uint _offset;
            private uint _compressedLength;
            private uint _originalLength;
            private byte[] _unknown1;
            private uint _compressionType;
            private byte[] _unknown2;
            private Pak m_root;
            private Pak.PakEntry m_parent;
            public uint Offset { get { return _offset; } }
            public uint CompressedLength { get { return _compressedLength; } }
            public uint OriginalLength { get { return _originalLength; } }
            public byte[] Unknown1 { get { return _unknown1; } }
            public uint CompressionType { get { return _compressionType; } }
            public byte[] Unknown2 { get { return _unknown2; } }
            public Pak M_Root { get { return m_root; } }
            public Pak.PakEntry M_Parent { get { return m_parent; } }
        }
        private byte _entryType;
        private byte _nameLength;
        private string _name;
        private KaitaiStruct _info;
        private Pak m_root;
        private KaitaiStruct m_parent;
        public byte EntryType { get { return _entryType; } }
        public byte NameLength { get { return _nameLength; } }
        public string Name { get { return _name; } }
        public KaitaiStruct Info { get { return _info; } }
        public Pak M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
    public partial class RawChunk : KaitaiStruct
    {
        public static RawChunk FromFile(string fileName)
        {
            return new RawChunk(new KaitaiStream(fileName));
        }

        public RawChunk(KaitaiStream p__io, Pak.Chunk p__parent = null, Pak p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            f_unknown = false;
            _read();
        }
        private void _read()
        {
        }
        private bool f_unknown;
        private byte[] _unknown;
        public byte[] Unknown
        {
            get
            {
                if (f_unknown)
                    return _unknown;
                long _pos = m_io.Pos;
                m_io.Seek(0);
                _unknown = m_io.ReadBytesFull();
                m_io.Seek(_pos);
                f_unknown = true;
                return _unknown;
            }
        }
        private Pak m_root;
        private Pak.Chunk m_parent;
        public Pak M_Root { get { return m_root; } }
        public Pak.Chunk M_Parent { get { return m_parent; } }
    }
    public partial class DataChunk : KaitaiStruct
    {
        public static DataChunk FromFile(string fileName)
        {
            return new DataChunk(new KaitaiStream(fileName));
        }

        public DataChunk(KaitaiStream p__io, Pak.Chunk p__parent = null, Pak p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            f_content = false;
            _read();
        }
        private void _read()
        {
        }
        private bool f_content;
        private byte[] _content;
        public byte[] Content
        {
            get
            {
                if (f_content)
                    return _content;
                long _pos = m_io.Pos;
                m_io.Seek(0);
                _content = m_io.ReadBytesFull();
                m_io.Seek(_pos);
                f_content = true;
                return _content;
            }
        }
        private Pak m_root;
        private Pak.Chunk m_parent;
        public Pak M_Root { get { return m_root; } }
        public Pak.Chunk M_Parent { get { return m_parent; } }
    }
    public partial class FileChunk : KaitaiStruct
    {
        public static FileChunk FromFile(string fileName)
        {
            return new FileChunk(new KaitaiStream(fileName));
        }

        public FileChunk(KaitaiStream p__io, Pak.Chunk p__parent = null, Pak p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _root = new PakEntry(m_io, this, m_root);
        }
        private PakEntry _root;
        private Pak m_root;
        private Pak.Chunk m_parent;
        public PakEntry Root { get { return _root; } }
        public Pak M_Root { get { return m_root; } }
        public Pak.Chunk M_Parent { get { return m_parent; } }
    }
    private byte[] _form;
    private uint _length;
    private byte[] _formType;
    private List<Chunk> _chunks;
    private Pak m_root;
    private KaitaiStruct m_parent;
    public byte[] Form { get { return _form; } }
    public uint Length { get { return _length; } }
    public byte[] FormType { get { return _formType; } }
    public List<Chunk> Chunks { get { return _chunks; } }
    public Pak M_Root { get { return m_root; } }
    public KaitaiStruct M_Parent { get { return m_parent; } }
}
