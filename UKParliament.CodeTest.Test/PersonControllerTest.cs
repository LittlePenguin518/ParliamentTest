using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Models;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using OkResult = Microsoft.AspNetCore.Mvc.OkResult;

namespace UKParliament.CodeTest.Test
{
    [TestClass]
    public class PersonControllerTest
    {
        
        //private readonly ControllerBase _personService;
        
        private readonly IPersonService _personService;
        private readonly PersonController PersonController;

        public PersonControllerTest()
        {
           
            PersonController = new PersonController(_personService);
        }

        [Fact]
        public void ShouldGetPerson()
        {
            // arrange
            int viewPersonId = 1;

            //act
            var result = PersonController.GetPerson(viewPersonId);

            // assert
            Assert.IsNotNull(result);
            
           


        }
       
        [Fact]
        public void ShouldAddPerson()
        {
            // arrange
            PersonInfo RequestAddPerson = new PersonInfo
            {
                Id = 1,
                Name = "TestDummy1",
                DateOfBirth = "04/04/2004"

            };

            // act
            var actionResult = PersonController.CreatePerson(RequestAddPerson);
           
            // assert
            Assert.IsNotNull(actionResult);
        


        }


        [Fact]
        public void ShouldUpdatePerson()
        {
        
            // arrange
            PersonInfo RequestViewPerson = new PersonInfo
            {
                Id = 1,
                Name = "TestDummy1",
                DateOfBirth = "04/04/2004"

            };

            //act
            
            var result = PersonController.UpdatePerson(RequestViewPerson);

            // assert
            Assert.IsNotNull(result);
           


        }

        [Fact]
        public void ShouldDeletePerson()
        {
            // arrange
            int PersonId = 1;

            //act
            var result = PersonController.Delete(PersonId);

            // assert
            Assert.IsNotNull(result);
           




        }
    }
}
