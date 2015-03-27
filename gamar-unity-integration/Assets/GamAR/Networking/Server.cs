using UnityEngine;
using System.Collections;
using System;
using System.IO;

using GamAR.Networking.Responses;

namespace GamAR.Networking
{
    public class Server
    {
        public string[] MarkerNames { get; private set; }
        public string[] MarkerKeys { get; private set; }

        public MonoBehaviour Behaviour { get; private set; }
        public string ApiURL { get; private set; }

        public string SessionId = "";
        private User user;

        public string ErrorText { get; private set; }
        public int ErrorId { get; private set; }

        public bool Connected { get { return SessionId != string.Empty; } }

        public bool Loading { get; private set; }

        public CheckinResponse CurrentCheckin { get; private set; }

        public Server(string url, MonoBehaviour mb)
        {
            ApiURL = url;
            Behaviour = mb;

            MarkerNames = new string[] { "Image Marker", "Apple iBeacon", "Hash" };
            MarkerKeys  = new string[] { "image_marker", "ibeacon",       "hash" };

            Loading = false;

            user = new User();
        }

        #region score
        public void Score(int value, Action<ScoreResponse> callback)
        {
            ErrorText = "";
            ErrorId = 0;

            Behaviour.StartCoroutine(score(value, callback));
        }

        private IEnumerator score(int value, Action<ScoreResponse> callback)
        {
            Request r = new Request("game/score");
            r.AddString("score", value.ToString());

            WWW w = runRequest(r);
            yield return w; // wait for response

            ScoreResponse res = new ScoreResponse(processWWWResult(w));
            if (!res.Ready)
            {
                res.ErrorId = ErrorId;
                res.ErrorText = ErrorText;
            }
            else CurrentCheckin = null;

            callback(res);
        }
        #endregion

        #region check-out
        public void Checkout(Action<CheckoutResponse> callback)
        {
            ErrorText = "";
            ErrorId = 0;

            Behaviour.StartCoroutine(checkout(callback));
        }

        private IEnumerator checkout(Action<CheckoutResponse> callback)
        {
            Request r = new Request("game/checkout");

            WWW w = runRequest(r);
            yield return w; // wait for response

            CheckoutResponse res = new CheckoutResponse(processWWWResult(w));
            if (!res.Ready)
            {
                res.ErrorId = ErrorId;
                res.ErrorText = ErrorText;
            }
            else CurrentCheckin = null;

            callback(res);
        }
        #endregion

        #region check-in
        public void Checkin(int markerId, string markerValue, Action<CheckinResponse> callback)
        {
            ErrorText = "";
            ErrorId = 0;

            string key = MarkerKeys[markerId];

            Behaviour.StartCoroutine(checkin(key, markerValue, callback));
        }

        private IEnumerator checkin(string markerKey, string markerValue, Action<CheckinResponse> callback)
        {
            Request r = new Request("game/checkin");
            r.AddString("marker", markerKey);
            r.AddString("value", markerValue);

            WWW w = runRequest(r);
            yield return w; // wait for response

            CheckinResponse res = new CheckinResponse(processWWWResult(w));
            if (!res.Ready)
            {
                res.ErrorId = ErrorId;
                res.ErrorText = ErrorText;
            }
            else CurrentCheckin = res;

            callback(res);
        }
        #endregion

        #region register
        public void Register(string groupCode, string name, string character, string deviceId, Action<User> callback)
        {
            Register(groupCode, name, character, "", "", deviceId, callback);
        }

        public void Register(string groupCode, string name, string character, string email, string password, string deviceId, Action<User> callback)
        {
            ErrorText = "";
            ErrorId = 0;

            user.GroupCode = groupCode;
            user.Name = name;
            user.Character = character;
            user.Email = email;
            user.Password = password;
            user.DeviceId = deviceId;

            Behaviour.StartCoroutine(register(callback));
        }

        private IEnumerator register(Action<User> callback)
        {
            Request r = new Request("register");
            r.AddString("code", user.GroupCode);
            r.AddString("name", user.Name);
            r.AddString("char", user.Character);
            r.AddString("email", user.Email);
            r.AddString("password", user.Password);
            r.AddString("device", user.DeviceId);

            WWW w = runRequest(r);
            yield return w; // wait for response

            processWWWResult(w, true); // autologin after register
            callback(user);
        }
        #endregion

        #region login by nickname and device ID
        public void Login(string nickname, string deviceId, Action<User> callback)
        {
            ErrorText = "";
            ErrorId = 0;

            user.Name = nickname;
            user.DeviceId = deviceId;

            Behaviour.StartCoroutine(login(callback));
        }

        private IEnumerator login(Action<User> callback)
        {
            Request r = new Request("login");
            r.AddString("name", user.Name);
            r.AddString("device", user.DeviceId);

            WWW w = runRequest(r);
            yield return w; // wait for response

            processWWWResult(w, true);
            callback(user);
        }
        #endregion

        #region login by email and password
        /*public void Login(string email, string password, Action<User> callback)
        {
            ErrorText = "";
            ErrorId = 0;

            user.Email = email;
            user.Password = password;

            Behaviour.StartCoroutine(login(callback));
        }

        private IEnumerator login(Action<User> callback)
        {
            Request r = new Request("login");
            r.AddString("email", user.Email);
            r.AddString("password", user.Password);

            WWW w = runRequest(r);
            yield return w; // wait for response

            processWWWResult(w, true);
            callback(user);
        }*/
        #endregion

        #region logout
        public void Logout(Action<User> callback)
        {
            ErrorText = "";
            ErrorId = 0;

            Behaviour.StartCoroutine(logout(callback));
        }

        private IEnumerator logout(Action<User> callback)
        {
            Request r = new Request("logout");

            WWW w = runRequest(r);
            yield return w; // wait for response

            if (w.error == null)
            {
                JSONNode n = decode(w.text);

                user.Validated = !n["success"].AsBool;
                if (!user.Validated) SessionId = "";
            }
            else Debug.LogError("Server error");

            Loading = false;
            callback(user);
        }
        #endregion

        // base
        private byte[] encode(string s)
        {
            Debug.Log("encoding: " + s);
            return System.Text.Encoding.UTF8.GetBytes(s);
        }

        private JSONNode decode(string str)
        {
            try
            {
                str = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(str));
                Debug.Log("decoding: " + str);
            }
            catch(Exception e)
            {
                str = "";
                Debug.LogError("decode error: " + e.Message);
            }

            JSONNode node = JSON.Parse(str);
            return node;
        }

        private WWW runRequest(Request data)
        {
            Loading = true;
            string path = data.Path;

            WWWForm form = new WWWForm();
            form.AddField("json", data.ToJSONString()); //TODO: encryption
            if (Connected) form.AddField("s", SessionId);

            return new WWW(ApiURL + Path.Combine(ApiURL, path), form);
        }

        private JSONNode processWWWResult(WWW r)
        {
            return processWWWResult(r, false);
        }

        private JSONNode processWWWResult(WWW r, bool isLogin)
        {
            Loading = false;

            if (r.error == null)
            {
                JSONNode n = decode(r.text);
                bool success = n["success"].AsBool;
                
                if (isLogin) 
                {
                    user.Validated = success;
                    if (success) SessionId = n["session"];
                }

                if (success) 
                {
                    ErrorText = "";
                    ErrorId = 0;
                    return n;
                }
                else
                {
                    ErrorText = n["message"];
                    ErrorId = n["error"].AsInt;
                }
            }
            else
            {
                Debug.LogError("Server error: " + r.text);
                ErrorText = "server error";
                ErrorId = -1;
            }

            return null;
        }

    }
}