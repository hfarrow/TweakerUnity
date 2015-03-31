using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostbit.Tweaker.Core
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
		/// Get the type of each argument this invokable requires when invoked.
		/// </summary>
		Type[] ArgTypes { get; }

		/// <summary>
		/// Invoke the invokable object with the provided arguments.
		/// Throws InvokeException, InvokeArgNumberException, InvokeArgTypeException
		/// </summary>
		/// <param name="args">Arguments to invoke with.</param>
		/// <returns>The return value of the invokable.</returns>
		object Invoke(params object[] args);

		//object InvokeWithArgArray(object[])

		/// <summary>
		/// The manager that this invokable is registered to.
		/// An invokable can only be registered to one manager at any given time.
		/// </summary>
		IInvokableManager Manager { get; set; }
	}
}
