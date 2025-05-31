using Kaitai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

namespace PakInspector;

public partial class Iff : KaitaiStruct
{
    public static Iff FromFile(string fileName)
    {
        return new Iff(new KaitaiStream(fileName));
    }

    public Iff(KaitaiStream p__io, KaitaiStruct p__parent = null, Iff p__root = null) : base(p__io)
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
        _formType = System.Text.Encoding.GetEncoding("ASCII").GetString(m_io.ReadBytes(4));
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

        public Chunk(KaitaiStream p__io, Iff p__parent = null, Iff p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _typeId = System.Text.Encoding.GetEncoding("ASCII").GetString(m_io.ReadBytes(4));
            _length = m_io.ReadU4be();
            _body = m_io.ReadBytes(Length);
        }
        private string _typeId;
        private uint _length;
        private byte[] _body;
        private Iff m_root;
        private Iff m_parent;
        public string TypeId { get { return _typeId; } }
        public uint Length { get { return _length; } }
        public byte[] Body { get { return _body; } }
        public Iff M_Root { get { return m_root; } }
        public Iff M_Parent { get { return m_parent; } }
    }
    private byte[] _form;
    private uint _length;
    private string _formType;
    private List<Chunk> _chunks;
    private Iff m_root;
    private KaitaiStruct m_parent;
    public byte[] Form { get { return _form; } }
    public uint Length { get { return _length; } }
    public string FormType { get { return _formType; } }
    public List<Chunk> Chunks { get { return _chunks; } }
    public Iff M_Root { get { return m_root; } }
    public KaitaiStruct M_Parent { get { return m_parent; } }
}
