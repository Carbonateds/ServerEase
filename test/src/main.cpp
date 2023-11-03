#include <iostream>
#include "property.h"
#include "configuration.h"

int main()
{
    std::cout << "Current Library Version: " << GetLibraryVersion() << std::endl;
    Initialize();
    return 0;
}
