using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour
{
    [SerializeField] private float maxDistance = 30f;
    [SerializeField] private int maxReflectionCount = 5;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        FireLaser();
    }

    private void FireLaser()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        line.positionCount = 1;
        line.SetPosition(0, origin);

        for (int i = 0; i < maxReflectionCount; i++)
        {
            Ray ray = new Ray(origin, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                line.positionCount++;
                line.SetPosition(line.positionCount -1, hit.point);

                ILasetInteractable interactable = hit.collider.GetComponent<ILasetInteractable>();

                interactable?.OnLaserHit();

                MirrorReflector mirror = hit.collider.GetComponent<MirrorReflector>();

                if(mirror != null)
                {
                    direction = Vector3.Reflect(direction, hit.normal);
                    origin = hit.point;
                    continue;
                }

                break;
            }
            else
            {
                line.positionCount++;
                line.SetPosition(line.positionCount - 1, origin + direction * maxDistance);

                break;
            }
        }
    }
}
