using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Vector3 origin = new Vector3(0, 1f, 0);
    
    void Start()
    {
        SetOrigin(new Vector3(0, 1f, 0));
        ResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ResetPosition()
    {
        transform.position = origin;
    }
    
    public void SetOrigin(Vector3 pos)
    {
        origin = pos;
    }
}
