using System;

namespace CS.DataLibrary.Repositories
{
    public interface IDeletedEntity
    {
        public bool IsDeleted { get; set; }
    }
}
