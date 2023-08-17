const apiUrl = "https://localhost:5001/api";

export const getExercisesByMuscleGroupId = (muscleGroupId) => {
  return fetch(`${apiUrl}/exercise/musclegroup/${muscleGroupId}`).then(
    (res) => {
      if (!res.ok) {
        throw new Error("nope");
      }
      return res.json();
    }
  );
};
export const getExerciseById = (exerciseId) => {
  return fetch(`${apiUrl}/Exercise/${exerciseId}`)
    .then((response) => {
      if (response.ok) {
        return response.json();
      } else {
        throw new Error("Failll");
      }
    })
    .then((data) => {
      console.log("no luck", data);
      return data;
    });
};
export const addExercise = (exercise) => {
  return fetch(`${apiUrl}/exercise`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(exercise),
  }).then((res) => res.json());
};

export const updateExercise = (id, exercise) => {
  return fetch(`${apiUrl}/exercise/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(exercise),
  });
};

export const deleteExercise = (id) => {
  return fetch(`${apiUrl}/exercise/${id}`, {
    method: "DELETE",
  });
};
