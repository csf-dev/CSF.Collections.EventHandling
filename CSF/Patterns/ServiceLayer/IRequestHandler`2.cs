//
//  IRequestHander.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2012 Craig Fowler
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;

namespace CSF.Patterns.ServiceLayer
{
  /// <summary>
  /// Interface for a request handler - a type that handles requests and returns responses (or optionally does not
  /// return any form of response).
  /// </summary>
  /// <remarks>
  /// <para>
  /// See the documentation remarks for the non-generic <see cref="IRequestHandler"/> for important information about
  /// the required architecture of request handlers.
  /// </para>
  /// </remarks>
  public interface IRequestHandler<TRequest,TResponse> : IRequestHandler
    where TRequest : IRequest
    where TResponse : IResponse
  {
    /// <summary>
    /// Handles a standard/typical request, in which a response is returned to the caller.
    /// </summary>
    /// <param name='request'>
    /// The request to handle.
    /// </param>
    TResponse Handle(TRequest request);

    /// <summary>
    /// Handles a one-way/fire-and-forget request.  This method does not return any kind of response.
    /// </summary>
    /// <param name='request'>
    /// The request to handle.
    /// </param>
    void HandleRequestOnly(TRequest request);
  }
}

