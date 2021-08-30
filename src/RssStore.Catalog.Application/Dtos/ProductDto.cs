using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RssStore.Catalog.Application.Dtos
{
    public class ProductDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Name { get;  set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public bool Active { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime RegisterDate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Image { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor mínimo de {1}")]
        public int StockAmount { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor mínimo de {1}")]
        public decimal Width { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor mínimo de {1}")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor mínimo de {1}")]
        public decimal Depth { get; set; }

        public IEnumerable<CategoryDto> Categories { get; set; }
    }
}
