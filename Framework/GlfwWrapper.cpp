#include "Framework/GlfwWrapper.h"

#include <iostream>

void GlfwWrapper::Init(const std::string& window_name, uint32_t window_width, uint32_t window_height)
{
	// glfw: initialize and configure
	// ------------------------------
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
#ifdef __APPLE__
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
#endif

	// glfw window creation
	// --------------------
	m_window = glfwCreateWindow(int(window_width), int(window_height), window_name.c_str(), NULL, NULL);
	if (m_window == NULL)
	{
		std::cout << "Failed to create GLFW window" << std::endl;
		glfwTerminate();
		return;
	}
	glfwMakeContextCurrent(m_window);
	glfwSetWindowUserPointer(m_window, reinterpret_cast<void*>(this));

	// glad: load all OpenGL function pointers
	// ---------------------------------------
	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		std::cout << "Failed to initialize GLAD" << std::endl;
		return;
	}

	// callbacks
	// ---------------------------------------
	glfwSetFramebufferSizeCallback(m_window, [](GLFWwindow* window, int width, int height) {
		glViewport(0, 0, width, height);
	});

}

void GlfwWrapper::ShutDown()
{
	glfwTerminate();
}


