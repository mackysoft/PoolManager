using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MackySoft.Pooling {
	public class PoolException : Exception {

		public PoolException () : base() { }

		public PoolException (string message) : base(message) { }

		public PoolException (string message,Exception innerException) : base(message,innerException) { }

	}
}