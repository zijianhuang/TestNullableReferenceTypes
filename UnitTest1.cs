using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace NullableReferenceTypes
{
	public class Misc
	{
		[Fact]
		public void TestNotNullAttributeOnNullableEnabled()
		{
			Type methodContainerType = Type.GetType("NullableReferenceTypes.Misc");
			Assert.NotNull(methodContainerType);
			var methodBase = methodContainerType.GetMethod("GetNotNullStringOnNullableEnabled");
			Assert.NotNull(methodBase);
			var customAttributes = methodBase.CustomAttributes.ToArray();
			Assert.NotEmpty(customAttributes);
			Assert.Equal((byte)1, customAttributes[0].ConstructorArguments[0].Value); // return type string is not with ?
			Assert.Equal("System.Runtime.CompilerServices.NullableContextAttribute", customAttributes[0].AttributeType.FullName);
		}

		[Fact]
		public void TestNullAttributeOnNullableEnabled()
		{
			Type methodContainerType = Type.GetType("NullableReferenceTypes.Misc");
			Assert.NotNull(methodContainerType);
			MethodInfo methodBase = methodContainerType.GetMethod("GetNullStringOnNullableEnabled");
			Assert.NotNull(methodBase);
			var customAttributes = methodBase.CustomAttributes.ToArray();
			Assert.NotEmpty(customAttributes);
			//For compiler only.
			//I can see it in IDE Debugger, but not available in codes according to https://github.com/dotnet/roslyn/blob/main/docs/features/nullable-metadata.md.
			Assert.Equal((byte)2, customAttributes[0].ConstructorArguments[0].Value); // return type string is with ?
			Assert.Equal("System.Runtime.CompilerServices.NullableContextAttribute", customAttributes[0].AttributeType.FullName);


			Assert.False(Attribute.IsDefined(methodBase.ReturnParameter, typeof(System.Diagnostics.CodeAnalysis.NotNullAttribute)));
		}

		[Fact]
		public void TestGetA()
		{
			Type methodContainerType = Type.GetType("NullableReferenceTypes.Misc");
			Assert.NotNull(methodContainerType);
			var methodBase = methodContainerType.GetMethod("GetA");
			Assert.NotNull(methodBase);
			var customAttributes = methodBase.CustomAttributes.ToArray();
			Assert.NotEmpty(customAttributes); //[[System.Runtime.CompilerServices.NullableContextAttribute((Byte)2)]]
			Assert.Equal((byte)2, customAttributes[0].ConstructorArguments[0].Value); // return type A is with ?
			Assert.Equal("System.Runtime.CompilerServices.NullableContextAttribute", customAttributes[0].AttributeType.FullName);
		}

		[Fact]
		public void TestGetB()
		{
			Type methodContainerType = Type.GetType("NullableReferenceTypes.Misc");
			Assert.NotNull(methodContainerType);
			var methodBase = methodContainerType.GetMethod("GetB");
			Assert.NotNull(methodBase);
			var customAttributes = methodBase.CustomAttributes.ToArray();
			Assert.NotEmpty(customAttributes); //[[System.Runtime.CompilerServices.NullableContextAttribute((Byte)2)]]
			Assert.Equal((byte)1, customAttributes[0].ConstructorArguments[0].Value); // return type A is with ?
			Assert.Equal("System.Runtime.CompilerServices.NullableContextAttribute", customAttributes[0].AttributeType.FullName);
		}

		[Fact]
		public void TestAthletheSearch()
		{
			Type methodContainerType = Type.GetType("NullableReferenceTypes.Misc");
			Assert.NotNull(methodContainerType);
			var methodBase = methodContainerType.GetMethod("AthletheSearch");
			Assert.NotNull(methodBase);
			var customAttributes = methodBase.CustomAttributes.ToArray();
			Assert.NotEmpty(customAttributes); //[[System.Runtime.CompilerServices.NullableContextAttribute((Byte)1)]]
			Assert.Equal((byte)1, customAttributes[0].ConstructorArguments[0].Value); // return type string is not with ?
			Assert.Equal("System.Runtime.CompilerServices.NullableContextAttribute", customAttributes[0].AttributeType.FullName);
		}

		[return: System.Diagnostics.CodeAnalysis.NotNull]
		public string GetNotNullString()
		{
			if (DateTime.Now > new DateTime(2008, 12, 3))
			{
				return "ABCD";
			}
			else
			{
				return null;
			}
		}

#nullable enable
		public string GetNotNullStringOnNullableEnabled(string? sort)
		{
			return "ABCD";
		}

		public string? GetNullStringOnNullableEnabled()
		{
			if (DateTime.Now > new DateTime(2008, 12, 3))
			{
				return "ABCD";
			}
			else
			{
				return null;
			}
		}

		public A? GetA()
		{
			if (DateTime.Now > new DateTime(2008, 12, 3))
			{
				return new A();
			}
			else if (DateTime.Now > new DateTime(2008, 11, 3))
			{
				return new B();
			}

			return null;
		}

		public B GetB()
		{
			return new B();
		}

		public string AthletheSearch(string? sort, string? search)
		{
			return "ABCD";
		}

#nullable disable
		public class Base
		{
			public string P1 { get; set; }
		}

		public class A : Base
		{
			public string P2 { get; set; }
		}

		public class B : A
		{
			public string P3 { get; set; }
		}
	}

}