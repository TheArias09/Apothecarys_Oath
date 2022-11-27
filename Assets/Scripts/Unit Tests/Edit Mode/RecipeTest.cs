using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitTests
{
    public class RecipeTest
    {
        [Test]
        public void TestIngredient1()
        {
            List<Ingredient> ingredients = new()
            {
                new Ingredient("citron", 2),
                new Ingredient("sucre", 2, new List<IngredientState>() { IngredientState.CRUSHED }),
                new Ingredient("menthe", 1),
                new Ingredient("sirop", 0.1f),
                new Ingredient("eau gazeuse", 5)
            };

            Recipe recipe = new("Mojito", ingredients, new List<IngredientState>() { IngredientState.MIXED });

            Ingredient potion = new(1, ingredients);
            potion.AddState(IngredientState.MIXED);

           Assert.AreEqual(1, Recipe.CheckPotion(recipe, potion));
        }
    }
}
