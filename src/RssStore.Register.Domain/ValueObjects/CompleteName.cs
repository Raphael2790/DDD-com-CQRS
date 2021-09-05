﻿namespace RssStore.Register.Domain.ValueObjects
{
    public class CompleteName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
