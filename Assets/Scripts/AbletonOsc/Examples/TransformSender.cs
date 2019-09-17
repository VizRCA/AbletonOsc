using System;
using UnityEngine;
using uOSC;
using System.Linq;

namespace AbletonOsc.Examples
{
    public class TransformSender : MonoBehaviour
    {
        [Header("OSC")]
        public int objectId;
        public string address = "/object";
        public float frequency = 50;
        
        [Header("Position")]
        public bool sendPosition = true;
        public Vector3 posInMin = new Vector3(-5,-5,-5);
        public Vector3 posInMax = new Vector3(5,5,5);
        public Vector3 posOutMin = new Vector3(0,0,0);
        public Vector3 posOutMax = Vector3.one;
        
        [Header("Rotation")]
        public bool sendRotation = true;
        public Vector3 rotInMin = new Vector3(-360,-360,-360);
        public Vector3 rotInMax = new Vector3(360,360,360);
        public Vector3 rotOutMin = new Vector3(-1,-1,-1);
        public Vector3 rotOutMax = new Vector3(1,1,1);
        
        [Header("Scale")]
        public bool sendScale = true;
        public Vector3 sclInMin = new Vector3(0.001f,0.001f,0.001f);
        public Vector3 sclInMax = new Vector3(10, 10, 10);
        public Vector3 sclOutMin = Vector3.zero;
        public Vector3 sclOutMax = new Vector3(127,127,127);
        
        private Bundle _bundle;
        private Message _position;
        private Message _rotation;
        private Message _scale;

        private float _tF;
        private float _tD;

		private void OnEnable()
		{
            _tF = 1f / frequency;
            _tD = 0;
		}

		private void PrepBundle()
        {
            _bundle = new Bundle(Timestamp.Now);

            // You have to add each value separately so that it gets type tagged in the message
            // Scale all numbers to 0..1 range
            if (sendPosition)
            {
                var p = transform.position;
                p.x = Klak.Math.BasicMath.Map(p.x, posInMin.x, posInMax.x, posOutMin.x, posOutMax.x);
                p.y = Klak.Math.BasicMath.Map(p.y, posInMin.y, posInMax.y, posOutMin.y, posOutMax.y);
                p.z = Klak.Math.BasicMath.Map(p.z, posInMin.z, posInMax.z, posOutMin.z, posOutMax.z);
                _position = new Message(String.Format("{0}/{1}/position", address, objectId),
                    p.x, p.y, p.z);
                _bundle.Add(_position);
            }

            if (sendRotation)
            {
                var r = transform.rotation.eulerAngles;
                r.x = Klak.Math.BasicMath.Map(r.x, rotInMin.x, rotInMax.x, rotOutMin.x, rotOutMax.x);
                r.y = Klak.Math.BasicMath.Map(r.y, rotInMin.y, rotInMax.y, rotOutMin.y, rotOutMax.y);
                r.z = Klak.Math.BasicMath.Map(r.z, rotInMin.z, rotInMax.z, rotOutMin.z, rotOutMax.z);
                _rotation = new Message(String.Format("{0}/{1}/rotation", address, objectId),
                    r.x, r.y, r.z);
                _bundle.Add(_rotation);
            }


            if (sendScale)
            {
                var s = transform.localScale;
                s.x = Klak.Math.BasicMath.Map(s.x, sclInMin.x, sclInMax.x, sclOutMin.x,sclOutMax.x);
                s.y = Klak.Math.BasicMath.Map(s.y, sclInMin.y, sclInMax.y, sclOutMin.y,sclOutMax.y);
                s.z = Klak.Math.BasicMath.Map(s.z, sclInMin.z, sclInMax.z,  sclOutMin.z,sclOutMax.z);
                _scale = new Message(String.Format("{0}/{1}/scale", address, objectId),
                    s.x, s.y, s.z);
                _bundle.Add(_scale);
            }
        }

        private void FixedUpdate()
        {
            _tD += Time.fixedDeltaTime;
            if (_tD < _tF) return;
            _tD = 0;
            PrepBundle();
            LiveOscManager.Instance.Send(_bundle);
        }
    }
}