using Spring.Context;
using Spring.Objects.Factory.Support;

namespace guing
{
    public class Binder
    {
        private readonly IObjectDefinitionFactory objectDefinitionFactory;
        public Binder(IObjectDefinitionFactory objectDefinitionFactory)
        {
            this.objectDefinitionFactory = objectDefinitionFactory;
        }
    }
}
