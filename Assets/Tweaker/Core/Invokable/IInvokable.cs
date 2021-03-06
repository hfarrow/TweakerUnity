﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tweaker.Core
{
	/// <summary>
	/// All invokable tweaker objects implement this interface.
	/// </summary>
	public interface IInvokable : ITweakerObject
	{
		/// <summary>
		/// Get Info about this invokable such as description strings.
		/// </summary>
		InvokableInfo InvokableInfo { get; }

		/// <summary>
		/// Get the call signature of the method. ie: "void (int, string)"
		/// </summary>
		string MethodSignature { get; }

		/// <summary>
		/// Get the type of each parameter this invokable requires when invoked.
		/// </summary>
		Type[] ParameterTypes { get; }

		/// <summary>
		/// Get the parameters this invokable requires when invoked.
		/// </summary>
		ParameterInfo[] Parameters { get; }

		/// <summary>
		/// Invoke the invokable object with the provided arguments.
		/// Throws InvokeException, InvokeArgNumberException, InvokeArgTypeException
		/// </summary>
		/// <param name="args">Arguments to invoke with.</param>
		/// <returns>The return value of the invokable.</returns>
		object Invoke(params object[] args);

		/// <summary>
		/// The manager that this invokable is registered to.
		/// An invokable can only be registered to one manager at any given time.
		/// </summary>
		IInvokableManager Manager { get; set; }
	}
}
