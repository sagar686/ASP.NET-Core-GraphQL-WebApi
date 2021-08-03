using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace BookStoreGraphQLWebApi.GraphQL
{
    public class GraphSchema : Schema
    {
        public GraphSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<RootQuery>();
            //Mutation = new ChatMutation(chat);
            //Subscription = new ChatSubscriptions(chat);
        }
    }
}
