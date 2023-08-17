const apiUrl = "https://localhost:5001/api";

export const getAllTrainingPrograms = () => {
  return fetch(`${apiUrl}/trainingprogram`)
    .then((res) => res.json())
    .catch((error) => console.error("faiilleddd", error));
};

export const getTrainingProgramById = (id) => {
  return fetch(`${apiUrl}/trainingprogram/${id}`)
    .then((res) => res.json())
    .catch((error) => console.error(`faileddd ${id}:`, error));
};

export const addTrainingProgram = (trainingProgram) => {
  return fetch(`${apiUrl}/trainingprogram`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(trainingProgram),
  })
    .then((res) => res.json())
    .catch((error) => console.error("failureeee", error));
};

export const updateTrainingProgram = (trainingProgram) => {
  return fetch(`${apiUrl}/trainingprogram/${trainingProgram.Id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(trainingProgram),
  })
    .then((res) => res.json())
    .catch((error) => console.error("Nope", error));
};

export const deleteTrainingProgram = (id) => {
  return fetch(`${apiUrl}/trainingprogram/${id}`, {
    method: "DELETE",
  })
    .then((res) => res.json())
    .catch((error) => console.error("Error yo", error));
};
