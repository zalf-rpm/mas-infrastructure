#include <string>
#include <iostream>

#include <kj/string.h>

int main(int argc, char* argv[]) {
    std::cout << kj::str("hello world").cStr() << std::endl;
}
