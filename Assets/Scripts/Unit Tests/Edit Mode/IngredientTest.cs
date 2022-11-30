using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    public class IngredientTest
    {
        [Test]
        public void TestNull()
        {
            Ingredient ingredient = new("sucre", 1);
            Assert.AreNotEqual(null, ingredient);
        }

        [Test]
        public void TestName()
        {
            Ingredient ingredient1 = new("sucre", 1);
            Ingredient ingredient2 = new("viande", 1);
            Assert.AreNotEqual(ingredient1, ingredient2);
        }

        [Test]
        public void TestStates1()
        {
            Ingredient ingredient1 = new("sucre", 1);
            Ingredient ingredient2 = new("viande", 1);

            ingredient1.AddState(IngredientState.CRUSHED);
            Assert.AreNotEqual(ingredient1, ingredient2);
        }

        [Test]
        public void TestStates2()
        {
            Ingredient ingredient1 = new("sucre", 1);
            Ingredient ingredient2 = new("viande", 1);

            ingredient1.AddState(IngredientState.CRUSHED);
            ingredient2.AddState(IngredientState.MIXED);

            Assert.AreNotEqual(ingredient1, ingredient2);
        }

        [Test]
        public void TestQuantities()
        {
            Ingredient ingredient1 = new("sucre", 1);
            Ingredient ingredient2 = new("sucre", 2);

            Assert.AreEqual(ingredient1, ingredient2);
        }

        [Test]
        public void TestSuccess()
        {
            Ingredient ingredient1 = new("sucre", 1);
            Ingredient ingredient2 = new("sucre", 1);

            ingredient1.AddState(IngredientState.CRUSHED);
            ingredient2.AddState(IngredientState.CRUSHED);

            Assert.AreEqual(ingredient1, ingredient2);
        }
    }
}