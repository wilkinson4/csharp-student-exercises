using Newtonsoft.Json;
using StudentExercises.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestStudentExersises
{
    public class TestExerciseRoutes
    {
        [Fact]
        public async void Test_Get_All_Exercises()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/exercise");


                string responseBody = await response.Content.ReadAsStringAsync();
                var exerciseList = JsonConvert.DeserializeObject<List<Exercise>>(responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(exerciseList.Count > 0);
            }

        }

        [Fact]
        public async Task Test_Modify_Exercise()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                */
                Exercise modifiedExercise = new Exercise
                {
                    Name = "FlexBox",
                    Language = "CSS",
                };
                var modifiedExerciseAsJSON = JsonConvert.SerializeObject(modifiedExercise);

                var response = await client.PutAsync(
                    "/api/exercise/1",
                    new StringContent(modifiedExerciseAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);


                /*
                    GET section
                    Verify that the PUT operation was successful
                */
                var getExercise = await client.GetAsync("/api/exercise/1");
                getExercise.EnsureSuccessStatusCode();

                string getExerciseBody = await getExercise.Content.ReadAsStringAsync();
                Exercise newExercise = JsonConvert.DeserializeObject<Exercise>(getExerciseBody);

                Assert.Equal(HttpStatusCode.OK, getExercise.StatusCode);
                Assert.Equal(newExercise.Name, modifiedExercise.Name);
                Assert.Equal(newExercise.Language, modifiedExercise.Language);
            }
        }
        [Fact]
        public async Task Test_Create_Exercise()
        {
            /*
                Generate a new instance of an HttpClient that you can
                use to generate HTTP requests to your API controllers.
                The `using` keyword will automatically dispose of this
                instance of HttpClient once your code is done executing.
            */
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */

                // Construct a new student object to be sent to the API
                Exercise exercise = new Exercise
                {
                    Name = "Tuples",
                    Language = "Elixir"
                };

                // Serialize the C# object into a JSON string
                var exerciseAsJSON = JsonConvert.SerializeObject(exercise);


                /*
                    ACT
                */

                // Use the client to send the request and store the response
                var response = await client.PostAsync(
                    "/api/exercise",
                    new StringContent(exerciseAsJSON, Encoding.UTF8, "application/json")
                );

                // Store the JSON body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON into an instance of Animal
                var newExercise = JsonConvert.DeserializeObject<Exercise>(responseBody);


                /*
                    ASSERT
                */

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Tuples", newExercise.Name);
                Assert.Equal("Elixir", newExercise.Language);
            }
        }
        [Fact]
        public async Task Test_Delete_Exercise()
        {
            using (var client = new APIClientProvider().Client)
            {
                var getAllResponse = await client.GetAsync("/api/exercise");
                string responseBody = await getAllResponse.Content.ReadAsStringAsync();
                var exerciseList = JsonConvert.DeserializeObject<List<Exercise>>(responseBody);

                var deleteResponse = await client.DeleteAsync($"/api/exercise/{exerciseList.Count - 1}");
                string deleteResponseBody = await deleteResponse.Content.ReadAsStringAsync();
                Console.WriteLine(deleteResponseBody);

                var deletedItemResponse = await client.GetAsync($"api/exercise/{exerciseList.Count - 1}");


                Assert.Equal("Not Found", deletedItemResponse.ReasonPhrase);
            }
        }
    }
}
