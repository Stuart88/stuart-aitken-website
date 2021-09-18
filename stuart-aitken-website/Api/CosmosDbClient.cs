using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Shared.DbModels;

namespace API
{
    public class CosmosDbClient
    {
        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUri"];

        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "websites-db";
        private string containerId_Projects = "portfolio";

        // <GetStartedDemoAsync>
        /// <summary>
        /// Entry point to call methods that operate on Azure Cosmos DB resources in this sample
        /// </summary>
        public async Task Startup()
        {
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "stuart-aitken-website" });
          
        }
        // </GetStartedDemoAsync>



        // <AddItemsToContainerAsync>
        /// <summary>
        /// Add Family items to the container
        /// </summary>
        public async Task AddItemToContainerAsync(PortfolioProject p)
        {
            
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<PortfolioProject> projectResponse = await this.container.ReadItemAsync<PortfolioProject>(p.Id.ToString(), new PartitionKey(p.Id));
                Console.WriteLine("Item in database with id: {0} already exists\n", p.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<PortfolioProject> projectResponse = await this.container.CreateItemAsync<PortfolioProject>(p, new PartitionKey(p.Id));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", projectResponse.Resource.Id, projectResponse.RequestCharge);
            }
        }

        // <AddItemsToContainerAsync>
        /// <summary>
        /// Add Family items to the container
        /// </summary>
        public async Task UpdateItemInContainerAsync(PortfolioProject p)
        {

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<PortfolioProject> projectResponse = await this.container.ReadItemAsync<PortfolioProject>(p.Id.ToString(), new PartitionKey(p.Id));
                Console.WriteLine("Item in database with id: {0} already exists\n", p.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<PortfolioProject> projectResponse = await this.container.ReplaceItemAsync<PortfolioProject>(p, p.Id.ToString(), new PartitionKey(p.Id));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", projectResponse.Resource.Id, projectResponse.RequestCharge);
            }
        }
        // </AddItemsToContainerAsync>

        // <QueryItemsAsync>
        /// <summary>
        /// Run a query (using Azure Cosmos DB SQL syntax) against the container
        /// Including the partition key value of lastName in the WHERE filter results in a more efficient query
        /// </summary>
        public async Task<List<PortfolioProject>> QueryItemsAsync(int key)
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.PartitionKey = {key}";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<PortfolioProject> queryResultSetIterator = this.container.GetItemQueryIterator<PortfolioProject>(queryDefinition);

            List<PortfolioProject> results = new List<PortfolioProject>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<PortfolioProject> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (PortfolioProject family in currentResultSet)
                {
                    results.Add(family);
                    Console.WriteLine("\tRead {0}\n", family);
                }
            }

            return results;

        }
        // </QueryItemsAsync>


        // <DeleteFamilyItemAsync>
        /// <summary>
        /// Delete an item in the container
        /// </summary>
        public async Task DeleteFamilyItemAsync(PortfolioProject p)
        {

            // Delete an item. Note we must provide the partition key value and id of the item to delete
            ItemResponse<PortfolioProject> deletionResponse = await this.container.DeleteItemAsync<PortfolioProject>(p.Id.ToString(), new PartitionKey(p.Id));
            Console.WriteLine("Deleted Family [{0},{1}]\n", p.Id, p.Id);
        }
        // </DeleteFamilyItemAsync>

        // <DeleteDatabaseAndCleanupAsync>
        /// <summary>
        /// Delete the database and dispose of the Cosmos Client instance
        /// </summary>
        public async Task DeleteDatabaseAndCleanupAsync()
        {
            DatabaseResponse databaseResourceResponse = await this.database.DeleteAsync();
            // Also valid: await this.cosmosClient.Databases["FamilyDatabase"].DeleteAsync();

            Console.WriteLine("Deleted Database: {0}\n", this.databaseId);

            //Dispose of CosmosClient
            this.cosmosClient.Dispose();
        }
        // </DeleteDatabaseAndCleanupAsync>
    }
}
