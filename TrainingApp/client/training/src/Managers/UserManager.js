const apiUrl = "https://localhost:5001/api";

export const login = async (email, password) => {
  const response = await fetch(`${apiUrl}/user/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ email, password }),
  });

  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(errorData.message);
  }

  const data = await response.json();
  localStorage.setItem("token", data.token);

  return {
    token: data.token,
    userId: data.userId,
  };
};

export const logout = () => {
  localStorage.removeItem("token");
};

export const getToken = () => {
  return localStorage.getItem("token");
};

export const getUserById = async (id) => {
  const token = getToken();
  const response = await fetch(`${apiUrl}/user/${id}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failure");
  }

  return await response.json();
};

export const register = async (userDetails) => {
  const response = await fetch(`${apiUrl}/user`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(userDetails),
  });

  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(errorData.message);
  }

  return await response.json();
};

export const updateUser = async (user) => {
  const token = getToken();
  const response = await fetch(`${apiUrl}/user/${user.id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(user),
  });

  if (!response.ok) {
    throw new Error("Failed to update user details.");
  }

  if (response.status !== 204) {
    return await response.json();
  }
};
