using BookStoreGraphQLWebApi.GraphQL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.GraphiQL;

namespace BookStoreGraphQLWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddControllers();

            #region GraphQL
            services.AddScoped<GraphSchema>(); //dependency injection container
            services.AddGraphQL() //register all of the types that GraphQL.net uses
                .AddSystemTextJson() //Support serialization for GraphQL. Adds a GraphQL.Server.Transports.AspNetCore.IGraphQLRequestDeserializer
                .AddGraphTypes(typeof(GraphSchema), ServiceLifetime.Scoped); //scan the assembly and register all graph types such as the RootQuery and BookType types
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            #region GraphQL
            //Add the GraphQL middleware to the HTTP request pipeline
            //The path to the GraphQL endpoint which defaults to '/graphql'
            app.UseGraphQL<GraphSchema>();

            //GraphQL Playground is a graphical, interactive, in-browser GraphQL IDE,
            //created by Prisma and based on GraphiQL. 
            //localhost:XXXX/ui/playground
            app.UseGraphQLPlayground(new PlaygroundOptions());

            //GraphiQL is an interactive in-browser GraphQL IDE.
            //This is a fantastic developer tool to help you form queries and explore your Schema.
            //localhost:XXXX/ui/graphiql
            app.UseGraphQLGraphiQL();

            //Altair GraphQL Client is a beautiful feature-rich GraphQL Client IDE that enables you
            //interact with any GraphQL server you are authorized to access from any platform you are on.
            //You can easily test and optimize your GraphQL implementations.
            //You also have several features to make your GraphQL development process much easier including
            //subscriptions, query scaffolding, formatting, multiple languages, themes, and many more
            //localhost:XXXX/ui/altair
            app.UseGraphQLAltair();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
