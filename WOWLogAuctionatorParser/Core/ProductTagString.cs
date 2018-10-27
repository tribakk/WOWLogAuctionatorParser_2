using System;

namespace WOWLogAuctionatorParser.Core
{
    public class ProductTagString
    {
        public ProductTag m_Tag;
        public String m_Name;
        public ProductTagString(ProductTag tag, String name)
        {
            m_Tag = tag;
            m_Name = name;
        }
    };
}