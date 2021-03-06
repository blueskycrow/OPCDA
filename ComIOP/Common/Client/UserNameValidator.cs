/* ========================================================================
 * Copyright (c) 2005-2017 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Opc.Ua.Configuration;


namespace Opc.Ua.Com.Client
{
    /// <summary>
    /// Validates UserName.
    /// </summary>
    public class UserNameValidator
    {
        /// <summary>
        /// Triple DES Key
        /// </summary>
        private const string strKey = "h13h6m9F";

        /// <summary>
        /// Triple DES initialization vector
        /// </summary>
        private const string strIV = "Zse5";

        #region Constructors
        /// <summary>
        /// The default constructor.
        /// </summary>
        public UserNameValidator(string applicationName)
        {
            m_UserNameIdentityTokens = UserNameCreator.LoadUserName(applicationName);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Validates a User.
        /// </summary>
        /// <param name="token">UserNameIdentityToken.</param>
        /// <returns>True if the list contains a valid item.</returns>
        public bool Validate(UserNameIdentityToken token)
        {
            return Validate(token.UserName, token.DecryptedPassword);
        }

        /// <summary>
        /// Validates a User.
        /// </summary>
        /// <param name="name">user name.</param>
        /// <param name="password">password.</param>
        /// <returns>True if the list contains a valid item.</returns>
        public bool Validate(string name, string password)
        {
            lock (m_lock)
            {
                if (!m_UserNameIdentityTokens.ContainsKey(name))
                {
                    return false;
                }

                return (m_UserNameIdentityTokens[name].DecryptedPassword == password);
            }
        }

        #endregion

        #region Private Fields
        private object m_lock = new object();
        private Dictionary<string, UserNameIdentityToken> m_UserNameIdentityTokens = new Dictionary<string, UserNameIdentityToken>();
        #endregion
    }
}
