
namespace DCParry;

class AlwaysDestroy : MonoBehaviour
{
    private void OnDisable() {
        Destroy(gameObject);
    }
}
