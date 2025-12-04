using Unity.VisualScripting;
using UnityEngine;

public class IKRobotSystem : MonoBehaviour
{
    public Transform m_cartesian_transform_Z;
    public Transform m_cartesian_transform_Y;

    public Transform m_robot_root;
    public Transform m_gripper;

    public Transform m_gripped_gchroll;

    public PalleteSystem m_pallete_system;
    public GchRollSystem m_gchroll_system;

    public float m_cartesian_speed = 1.0f;
    public float m_gripping_time = 3.0f;

    private Vector3 get_gchroll_pos;
    private Vector3 set_gchroll_pos;


    private float duration;
    private float duration_current;


    private Vector3 start_cartesian_pos;
    private Vector3 end_cartesian_pos;

    private Vector3 start_gripper_pos;
    private Vector3 end_gripper_pos;

    private Quaternion start_gripper_rot;
    private Quaternion end_gripper_rot;

    private bool gripped_gchroll;
    private bool getset_pos;

    public Transform m_camera_lazor;
    private float duration_per
    {
        get
        {
            return duration_current / duration;
        }
    }
    private GCHROBOT_STATE robot_state;
    void Start()
    {
        duration = 1;
        duration_current = 1;
        robot_state = GCHROBOT_STATE.START;
        gripped_gchroll = false;
        getset_pos = false;
        m_gripped_gchroll.gameObject.SetActive(false);
        start_gripper_pos = Vector3.zero;
        end_gripper_pos = Vector3.zero;
        start_gripper_rot = m_gripper.transform.rotation;
        end_gripper_rot = m_gripper.transform.rotation;
        start_gripper_pos = m_gripper.transform.position;
        end_gripper_pos = m_gripper.transform.position;

        m_camera_lazor.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        duration_current = duration_current - Time.deltaTime;
        if (duration_current<0)
        {
            change_state();
            Debug.Log(robot_state);
        }
        else
        {
            move_update();
        }
    }
    private void change_state()
    {
        switch(robot_state)
        {
            case GCHROBOT_STATE.START:
                {
                    change_move_origin();
                    break;
                }
            case GCHROBOT_STATE.MOVE_PALLETE:
                {
                    change_set_gchroll();

                    break;
                }
            case GCHROBOT_STATE.MOVE_GCHROLL:
                {
                    change_get_gchroll();

                    break;
                }
            case GCHROBOT_STATE.GET_GCHROLL:
                {
                    gripped_gchroll = true;
                    m_gripped_gchroll.gameObject.SetActive(true);
                    m_gchroll_system.h_gchroll_gripped();
                   
                    change_move_origin();
                    break;
                }
            case GCHROBOT_STATE.SET_GCHROLL:
                {
                    gripped_gchroll = false;
                    getset_pos = false;
                    m_gripped_gchroll.gameObject.SetActive(false);
                    m_pallete_system.h_gchroll_setting();

                    change_move_origin();

                    m_camera_lazor.gameObject.SetActive(true);
                    break;
                }


            case GCHROBOT_STATE.MOVE_ORIGIN:
                {

                    m_camera_lazor.gameObject.SetActive(false);
                    if (getset_pos)
                    {
                        if(gripped_gchroll)
                        {
                            change_move_pallete();
                        }
                        else
                        {
                            change_move_gchroll();

                        }
                    }
                    else
                    {
                        gchroll_pos();
                        Debug.Log("gchroll_pos");
                        change_move_gchroll();

                    }
                    break;
                }
        }
    }
    private void change_move_pallete()
    {
        robot_state = GCHROBOT_STATE.MOVE_PALLETE;
        start_cartesian_pos = end_cartesian_pos;
        end_cartesian_pos = get_cartesian_pallete_pos;

        float distance = (start_cartesian_pos - end_cartesian_pos).magnitude;
        float time = distance / m_cartesian_speed;

        duration = time;
        duration_current = time;
    }
    private void change_move_gchroll()
    {
        robot_state = GCHROBOT_STATE.MOVE_GCHROLL;
        start_cartesian_pos = end_cartesian_pos;
        end_cartesian_pos = new Vector3(0, 0, get_gchroll_pos.z);

        float distance = (start_cartesian_pos - end_cartesian_pos).magnitude;
        float time = distance / m_cartesian_speed;
        Debug.Log(distance);

        duration = time;
        duration_current = time;
    }
    private void change_get_gchroll()
    {
        robot_state = GCHROBOT_STATE.GET_GCHROLL;
        start_gripper_pos = m_gripper.position;
        end_gripper_pos = get_gchroll_pos;
        start_gripper_rot = m_gripper.rotation;
        end_gripper_rot = Quaternion.Euler(0, 270, 0);

        duration = m_gripping_time;
        duration_current = m_gripping_time;
    }
    private void change_set_gchroll()
    {
        robot_state = GCHROBOT_STATE.SET_GCHROLL;
        start_gripper_pos = m_gripper.position;
        end_gripper_pos = set_gchroll_pos;
        start_gripper_rot = m_gripper.rotation;
        end_gripper_rot = Quaternion.Euler(-90, 180, 0);

        duration = m_gripping_time;
        duration_current = m_gripping_time;
    }
    private void change_move_origin()
    {
        robot_state = GCHROBOT_STATE.MOVE_ORIGIN;
        start_gripper_pos = m_gripper.position;
        end_gripper_pos = get_gripper_origin_pos; 
        start_gripper_rot = m_gripper.rotation;
        end_gripper_rot = Quaternion.Euler(0, 180, 0);

        duration = m_gripping_time;
        duration_current = m_gripping_time;
    }
    private void move_update()
    {
        switch(robot_state)
        {
            case GCHROBOT_STATE.START:
                break;
            case GCHROBOT_STATE.MOVE_PALLETE:
                m_cartesian_transform_Y.transform.position =
                    new Vector3(0, Vector3.Lerp(start_cartesian_pos, end_cartesian_pos, 1-duration_per).y,
                    Vector3.Lerp(start_cartesian_pos, end_cartesian_pos, 1 - duration_per).z);
                m_cartesian_transform_Z.transform.position =
                    new Vector3(0,0,Vector3.Lerp(start_cartesian_pos, end_cartesian_pos, 1-duration_per).z);

                break;
            case GCHROBOT_STATE.MOVE_GCHROLL:

                m_cartesian_transform_Y.transform.position =
                    new Vector3(0, Vector3.Lerp(start_cartesian_pos, end_cartesian_pos, 1-duration_per).y,
                    Vector3.Lerp(start_cartesian_pos, end_cartesian_pos, 1 - duration_per).z);
                m_cartesian_transform_Z.transform.position =
                    new Vector3(0, 0, Vector3.Lerp(start_cartesian_pos, end_cartesian_pos, 1-duration_per).z);
                break;
            case GCHROBOT_STATE.GET_GCHROLL:
                m_gripper.position = Vector3.Slerp(start_gripper_pos, end_gripper_pos, 1-duration_per);
                m_gripper.rotation = Quaternion.Slerp(start_gripper_rot, end_gripper_rot, 1-duration_per);
                break;
            case GCHROBOT_STATE.SET_GCHROLL:
                m_gripper.position = Vector3.Slerp(start_gripper_pos, end_gripper_pos, 1-duration_per);
                m_gripper.rotation = Quaternion.Slerp(start_gripper_rot, end_gripper_rot, 1-duration_per);
                break;
            case GCHROBOT_STATE.MOVE_ORIGIN:
                m_gripper.position = Vector3.Slerp(start_gripper_pos, end_gripper_pos, 1-duration_per);
                m_gripper.rotation = Quaternion.Slerp(start_gripper_rot, end_gripper_rot,1-duration_per);
                break;
        }
    }


    private Vector3 get_gripper_origin_pos
    {
        get
        {
            return m_robot_root.transform.position + new Vector3(0f, 1f, 0f);
        }
    }
    private Vector3 get_cartesian_pallete_pos
    {
        get
        {
            return new Vector3(0, 0.8f, 2.2f);
        }
    }
    private void gchroll_pos()
    {
        get_gchroll_pos = m_gchroll_system.h_get_gchroll_pos();
        set_gchroll_pos = m_pallete_system.h_get_gchroll_pos();
        getset_pos = true;
    }

    public enum GCHROBOT_STATE
    {
        START,

        GET_GCHROLL,
        MOVE_PALLETE,
        MOVE_GCHROLL,
        SET_GCHROLL,
        MOVE_ORIGIN,
    }
}