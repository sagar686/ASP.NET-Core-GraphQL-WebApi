using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace BookStoreGraphQLWebApi.GraphQL
{
    public class GraphSchema : Schema
    {
        public GraphSchema(IServiceProvider provider) : base(provider)
        {
            Query = new RootQuery();
            //Mutation = new ChatMutation(chat);
            //Subscription = new ChatSubscriptions(chat);
        }
    }
}
