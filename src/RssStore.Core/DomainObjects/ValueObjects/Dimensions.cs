using RssStore.Core.DomainObjects.Validations;

namespace RssStore.Core.DomainObjects.ValueObjects
{
    //um Value Object é imutavel sempre criamos novas intancias e sua criação é de responsabilidade da entidade que a possui
    public class Dimensions
    {
        public Dimensions(decimal width, decimal height, decimal depth)
        {
            AssertionConcern.ValidateIsLessThen(width, 1, "O campo largura não pode ser menor que 1cm");
            AssertionConcern.ValidateIsLessThen(height, 1, "O campo altura não pode ser menor que 1 cm");
            AssertionConcern.ValidateIsLessThen(depth, 1, "O campo profundidade não pode ser menor que 1 cm");

            Width = width;
            Height = height;
            Depth = depth;
        }

        public decimal Width { get; private set; }
        public decimal Height { get; private set; }
        public decimal Depth { get; private set; }

        public string FormatedDescription()
        {
            return $"LxAxP: {Width} x {Height} x {Depth}";
        }

        public override string ToString()
        {
            return FormatedDescription();
        }
    }
}
