const apiUrl = "https://localhost:5001/api";

export const getAllMuscleGroups = () => {
  return fetch(`${apiUrl}/musclegroup`).then((res) => res.json());
};

export const getMuscleGroupById = (id) => {
  return fetch(`${apiUrl}/musclegroup/${id}`).then((res) => res.json());
};
