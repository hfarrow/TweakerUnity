using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.Core;

public class ExampleNodes
{
	#region Root
	[Tweakable("String")]
	public static string root_string = "root_string_value";

	[Tweakable("Boolean")]
	public static bool root_bool = false;

	[Tweakable("Integer")]
	public static int root_int = 1;

	[Tweakable("Float")]
	public static float root_float = 1.0f;

	[Invokable("Command")]
	public static void root_command()
	{
		LogManager.GetCurrentClassLogger().Trace("root_command invoked");
	}

	#region Group Dog
	[Tweakable("Dog.Name")]
	public static string dog_name = "Spot";

	[Tweakable("Dog.Breed")]
	public static string dog_breed = "Shepard";

	[Tweakable("Dog.Size")]
	public static int dog_size = 5;

	#region Group Dog Toys
	[Tweakable("Dog.Toys.Favorite")]
	public static string dog_toys_favorite = "Bone";

	[Tweakable("Dog.Toys.Largest")]
	public static string dog_toys_largest = "Teddy Bear";

	[Tweakable("Dog.Toys.Smallest")]
	public static string dog_toys_smallest = "Rubber Ring";

	[Tweakable("Dog.Toys.Cheapest")]
	public static string dog_toys_cheapest = "Rope";

	[Tweakable("Dog.Toys.Placeholder1")]
	public static string dog_toys_placeholder1 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder2")]
	public static string dog_toys_placeholder2 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder3")]
	public static string dog_toys_placeholder3 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder4")]
	public static string dog_toys_placeholder4 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder5")]
	public static string dog_toys_placeholder5 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder6")]
	public static string dog_toys_placeholder6 = "placeholder";

	[Invokable("Dog.Toys.PlayWithToys")]
	public static void dog_toys_playWithToys(string toysToPlayWith)
	{

	}
	#endregion // Group Dog Toys
	#endregion // Group Dog

	#region Group Cat
	[Tweakable("Cat.Name")]
	public static string cat_name = "Fluffy";

	[Tweakable("Cat.Breed")]
	public static string cat_breed = "Alley";

	[Tweakable("Cat.Size")]
	public static int cat_size = 2;
	#endregion // Group Cat

	#endregion // Root
}
