using RssStore.Register.Domain.Enums;
using System;

namespace RssStore.Register.Domain.ValueObjects
{
    public class IdentificationDocument
    {
        public string DocumentNumber { get; set; }
        public DocumentType DocumentType { get; set; }
        public DateTime ExpirationDate { get; set; }

        public bool IsValid()
        {
            //logica para validar documento
            return true;
        }

        public bool IsExpired() => ExpirationDate < DateTime.Now;
    }

}
