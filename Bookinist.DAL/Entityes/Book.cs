using Bookinist.DAL.Entityes.Base;
using System;

namespace Bookinist.DAL.Entityes;

public class Book : NamedEntity
{
    public virtual Category Category { get; set; }

    public override string ToString() => $"Книга {Name}";
}

