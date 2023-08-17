const apiUrl = "https://localhost:5001/api";

export const addUserProgram = (userProgram) => {
  return fetch(`${apiUrl}/UserProgram`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(userProgram),
  }).then((response) => {
    if (!response.ok) {
      throw new Error("Still failin");
    }
    return response.json();
  });
};

export const getUserProgramByUserId = (userId) => {
  return fetch(`${apiUrl}/UserProgram/GetByUserId/${userId}`).then((response) =>
    response.json()
  );
};
