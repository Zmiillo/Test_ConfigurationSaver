using CS.DataLibrary.Repositories;
using System;

namespace CS.DataLibrary.Models
{
    public class BaseClass : IDeletedEntity, IIdEntity
    {
        public int Id { get; set; }        
        public bool IsDeleted { get; set; }       
    }
}
