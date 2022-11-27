using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    public class RecipeTest
    {
        [Test]
        public void TestAddedIngredient()
        {
            List<Ingredient> ingredients = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Recipe recipe = new("Mojito", new(ingredients), new() { IngredientState.MIXED });

            ingredients.Add(new Ingredient("viande", 4));

            Ingredient potion = new(1, ingredients);
            potion.AddState(IngredientState.MIXED);

            Assert.AreEqual(0, Recipe.CheckPotion(recipe, potion));
        }

        [Test]
        public void TestMissingIngredient()
        {
            List<Ingredient> ingredients = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Recipe recipe = new("Mojito", new(ingredients), new() { IngredientState.MIXED });

            ingredients.Remove(new Ingredient("menthe", 1));

            Ingredient potion = new(1, ingredients);
            potion.AddState(IngredientState.MIXED);

            Assert.AreEqual(0, Recipe.CheckPotion(recipe, potion));
        }

        [Test]
        public void TestMissingState1()
        {
            List<Ingredient> ingredients = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Recipe recipe = new("Mojito", new(ingredients), new() { IngredientState.MIXED });

            Ingredient potion = new(1, ingredients);
            //Potion is not mixed after the new ingredient is added

            Assert.AreEqual(0, Recipe.CheckPotion(recipe, potion));
        }

        [Test]
        public void TestMissingState2()
        {
            List<Ingredient> ingredients1 = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Recipe recipe = new("Mojito", ingredients1, new() { IngredientState.MIXED });

            List<Ingredient> ingredients2 = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Ingredient potion = new(1, ingredients2);
            potion.AddState(IngredientState.MIXED);

            Assert.AreEqual(0, Recipe.CheckPotion(recipe, potion));
        }

        [Test]
        public void TestAddedState1()
        {
            List<Ingredient> ingredients = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Recipe recipe = new("Mojito", new(ingredients), new() { IngredientState.MIXED });

            Ingredient potion = new(1, ingredients);
            potion.AddState(IngredientState.MIXED);
            potion.AddState(IngredientState.CRUSHED);

            Assert.AreEqual(0, Recipe.CheckPotion(recipe, potion));
        }

        [Test]
        public void TestAddedState2()
        {
            List<Ingredient> ingredients1 = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Recipe recipe = new("Mojito", ingredients1, new() { IngredientState.MIXED });

            List<Ingredient> ingredients2 = new()
            {
                new Ingredient("citron", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Ingredient potion = new(1, ingredients2);
            potion.AddState(IngredientState.MIXED);

            Assert.AreEqual(0, Recipe.CheckPotion(recipe, potion));
        }

        [Test]
        public void TestSuccessful()
        {
            List<Ingredient> ingredients = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Recipe recipe = new("Mojito", new(ingredients), new() { IngredientState.MIXED });

            Ingredient potion = new(1, ingredients);
            potion.AddState(IngredientState.MIXED);

            Assert.AreEqual(1, Recipe.CheckPotion(recipe, potion));
        }

        [Test]
        public void TestQuality1()
        {
            List<Ingredient> ingredients = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Recipe recipe = new("Mojito", new(ingredients), new() { IngredientState.MIXED });

            ingredients[0].SetQuality(0.5f);

            Ingredient potion = new(1, ingredients);
            potion.AddState(IngredientState.MIXED);

            Assert.AreNotEqual(0, Recipe.CheckPotion(recipe, potion));
            Assert.AreNotEqual(1, Recipe.CheckPotion(recipe, potion));
        }

        [Test]
        public void TestQuality2()
        {
            List<Ingredient> ingredients1 = new()
            {
                new Ingredient("citron", 1),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Recipe recipe = new("Mojito", ingredients1, new() { IngredientState.MIXED });

            List<Ingredient> ingredients2 = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Ingredient potion = new(1, ingredients2);
            potion.AddState(IngredientState.MIXED);

            Assert.AreNotEqual(0, Recipe.CheckPotion(recipe, potion));
            Assert.AreNotEqual(1, Recipe.CheckPotion(recipe, potion));
        }
    }
}
