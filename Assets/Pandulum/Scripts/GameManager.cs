using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//refact code
//use fixedjoint
//start to UI, like panel , so that user can start to interactive
//transform scaling, make it pecisely, position exactly , bar with capsule,

//fixed rodpos (ya, sort of...)
//crete tiny cube then connect with ball, tiny cube should fixed joint with rod
//try different drag  (slow down the object?)
//try different material like metalic one (ya, it's done, when "Bounce" set to 0.9-1.0)
//mass? and fixed joint really detail?

namespace Pendulum.Demo
{

    public class GameManager : MonoBehaviour
    {
        public GameObject soundManager;// which should assign to "AudioManger" in the inspector
        GameObject[] rodArray;
        GameObject[] sphereArray;
        GameObject mainBar;
        GameObject emptyObj;
        GameObject[] tinyObj;

        AudioSource colidSoundEff;

        int rodTotal;
        Rigidbody tempRB;//this is declareing, so without "=" !!
        float tempRadius;
        int swingballCount;

        Rigidbody mainBarRB;

        float ballMass;
        float rodMass;

        Material blkColorMat;
        PhysicMaterial metalSurfMat;

        void Start()
        {
            //CollisonSoundTest soundScript;
            //soundScript = soundManager.GetComponent<CollisonSoundTest>();

            rodTotal = 5;
            rodArray = new GameObject[rodTotal];
            tempRadius = 1.5f;

            swingballCount = 2;

            sphereArray = new GameObject[rodTotal];

            tinyObj = new GameObject[rodTotal];


            //float xGap = 0.6f;//it was the first attempt

            float firstRodXPos;

            float rodThickness;
            rodThickness = 0.02f;


            ballMass = 4.0f;
            rodMass = 0.5f;

            blkColorMat = Resources.Load("Materials/BlackColor", typeof(Material)) as Material;
            metalSurfMat = Resources.Load("Materials/MetalSurface", typeof(PhysicMaterial)) as PhysicMaterial;


            CreateMainBar();
            firstRodXPos = mainBar.transform.position.x - mainBar.transform.localScale.x / 2;

            //barRB = mainBar.AddComponent<Rigidbody>();//if I close it then it won't be mess, but still no
            //oject been connected



            for (int i = 0; i < rodTotal; i++)


            {

                rodArray[i] = SetRod(firstRodXPos);


                sphereArray[i] = SetSphere(rodArray[i], firstRodXPos, i);

                tinyObj[i] = SetTinyObj(rodArray[i], firstRodXPos);


                //sphereArray[i].AddComponent<AudioSource>();

                firstRodXPos += tempRadius + rodThickness;


            }

            rodArray[0].name = "Rod01";

            //for(int i=0; i < rodTotal; i++)
            //{
            //    rodArray[i].GetComponent<HingeJoint>().connectedBody = barRB;
            //}

            //sphereArray[0].GetComponent<CollisonSoundTest>


        }


        private void CreateMainBar()
        {

            mainBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mainBar.name = "Beam";
            mainBar.transform.localScale = new Vector3(20, 1, 1);
            mainBar.transform.position = new Vector3(5.8f, 10, 0);

            mainBarRB = mainBar.AddComponent<Rigidbody>();
            mainBarRB.useGravity = false;
            mainBarRB.isKinematic = true;

        }

        //private void SetSphere(GameObject hrBar)
        private GameObject SetSphere(GameObject refRod, float xPos, int indx)

        {
            GameObject refBall;

            Rigidbody rodRB;
            FixedJoint ballFJ;
            float refYforBall;
            float refY;

            //AudioSource colidSoundEff;

            refY = mainBar.transform.position.y - mainBar.transform.localScale.y / 2;  // y position of bottom of a bar 
            rodRB = refRod.GetComponent<Rigidbody>();

            //refYforBall = refY - refRod.transform.localScale.y * 2 - refRod.transform.localScale.y / 4;
            refYforBall = refY - refRod.transform.localScale.y * 2;
            refBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            refBall.transform.localScale = new Vector3(tempRadius, tempRadius, tempRadius);

            refBall.transform.position = new Vector3(xPos, refYforBall - tempRadius / 2.0f, mainBar.transform.position.z);

            refBall.GetComponent<Renderer>().material = blkColorMat;
            refBall.GetComponent<SphereCollider>().material = metalSurfMat;



            ballFJ = refBall.AddComponent<FixedJoint>();

            refBall.GetComponent<Rigidbody>().mass = ballMass;


            ballFJ.connectedBody = rodRB;

            /*refBall.AddComponent<CollisonSoundTest>();
            refBall.GetComponent<CollisonSoundTest>().index = indx;
            refBall.GetComponent<CollisonSoundTest>().audioManager = soundManager;*/ //just close for now
                                                                                     //it is working

            //colidSoundEff = refBall.AddComponent<AudioSource>();
            //colidSoundEff.clip= Resources.Load("AudioFiles/metalSou", typeof(AudioClip)) as AudioClip;
            //colidSoundEff.playOnAwake = (false);



            return refBall;
        }


        private GameObject SetRod(float xPos)

        {
            GameObject refRod;
            //GameObject refTinyObj;
            Rigidbody rodRB;
            //Rigidbody hrBarRB;
            HingeJoint rodHingeJoint;
            float refY;

            refY = mainBar.transform.position.y - mainBar.transform.localScale.y / 2;

            //refY = hrBar.transform.position.y - hrBar.transform.localScale.y / 2; //first
            //refY = hrBar.transform.position.y - hrBar.transform.localScale.x / 2;//when hrBar as cylinter rotate to 90"

            refRod = GameObject.CreatePrimitive(PrimitiveType.Capsule);


            emptyObj = new GameObject("rodsMom");
            refRod.transform.parent = emptyObj.transform;
            emptyObj.transform.position = new Vector3(xPos, refY, mainBar.transform.position.z);

            //refTinyObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            refRod.transform.localScale = new Vector3(0.02f, 2.8f, 0.02f);
            //rodForTest.transform.position = new Vector3(hrBar.transform.position.x, refY - rodForTest.transform.localScale.y, hrBar.transform.position.z);
            refRod.transform.position = new Vector3(xPos, refY - refRod.transform.localScale.y, mainBar.transform.position.z);
            //rodForTest.transform.position = new Vector3(hrBar.transform.position.x, hrBar.transform.position.y, hrBar.transform.position.z);

            //localScaleY is twice length for cyclinder and cpsusle,



            rodHingeJoint = refRod.AddComponent<HingeJoint>();

            rodHingeJoint.connectedBody = mainBarRB;

            Debug.Log("rodConnetedBody " + refRod.GetComponent<HingeJoint>().connectedBody);

            rodHingeJoint.axis = new Vector3(0f, 0f, 1f);
            //rodHingeJoint.autoConfigureConnectedAnchor = (false);
            //rodHingeJoint.connectedAnchor = new Vector3(xPos, refY, mainBar.transform.position.z);

            rodRB = refRod.GetComponent<Rigidbody>();
            rodRB.mass = rodMass;
            rodRB.useGravity = true;//turn it on, then it can swin
            rodRB.isKinematic = false;//could be a trigger when "false" is like real physics



            return refRod;

        }

        private GameObject SetTinyObj(GameObject refRod, float xPos)
        {
            GameObject refTinyObj;
            float refY;
            refY = mainBar.transform.position.y - mainBar.transform.localScale.y / 2;

            float refYforTinyObj;
            refYforTinyObj = refY - refRod.transform.localScale.y * 2;

            FixedJoint tinyFJ;
            Rigidbody refRodRB;

            refTinyObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            refTinyObj.transform.position = new Vector3(xPos, refYforTinyObj, mainBar.transform.position.z);
            refTinyObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            tinyFJ = refTinyObj.AddComponent<FixedJoint>();
            refRodRB = refRod.GetComponent<Rigidbody>();

            tinyFJ.connectedBody = refRodRB;

            return refTinyObj;
        }





        void Update()
        {
            if (Input.GetKeyUp("s"))
            {
                for (int i = 0; i < rodTotal; i++)
                {
                    tempRB = rodArray[i].GetComponent<Rigidbody>();//it is defining, and use "="!!
                    tempRB.useGravity = (true);
                }
            }

            if (Input.GetKeyUp("k"))
            {
                for (int i = 0; i < rodTotal; i++)
                {
                    tempRB = rodArray[i].GetComponent<Rigidbody>();//it is defining, and use "="!!
                    tempRB.isKinematic = (false);
                }
            }
            if (Input.GetKeyUp("a"))
            {
                for (int i = 0; i < swingballCount; i++)
                {
                    //tempRB = rodArray[i].GetComponent<Rigidbody>();
                    //tempRB.isKinematic = true;

                    //tempRB = sphereArray[i].GetComponent<Rigidbody>();
                    //tempRB.isKinematic = true;

                    //rodArray[i].transform.rotation = Quaternion.Euler(0, 0, -30);
                    rodArray[i].transform.parent.Rotate(0, 0, -30, Space.World);

                    //sphereArray[i].transform.rotation = Quaternion.Euler(0, 0, -30);

                }
            }




            if (Input.GetKeyUp("f"))
            {
                for (int i = 0; i < swingballCount; i++)
                {
                    tempRB = sphereArray[i].GetComponent<Rigidbody>();//it is defining, and use "="!!
                    tempRB.isKinematic = (false);
                    tempRB.AddForce(new Vector3(-3, 0, 0), ForceMode.Impulse);

                }

                //rodRB.isKinematic = (false);
            }


            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel("HingeByScript");
            }

        }

        
    }

}