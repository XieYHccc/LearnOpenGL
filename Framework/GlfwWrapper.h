#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <string>

class GlfwWrapper 
{
public:
	static GlfwWrapper& Get()
	{
		static GlfwWrapper instance;
		return instance;
	}

	GlfwWrapper() = default;
	~GlfwWrapper() = default;

	void Init(const std::string& window_name, uint32_t window_width, uint32_t window_height, uint32_t msaa_count = 1);
	void ShutDown();

	GLFWwindow* GetGLFWWindow() const { return m_window; }

private:
	GLFWwindow* m_window = nullptr;

};