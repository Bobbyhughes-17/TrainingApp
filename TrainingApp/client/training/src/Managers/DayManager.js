const apiUrl = "https://localhost:5001/api";

const dayApiUrl = `${apiUrl}/day`;

export const getDaysForTrainingProgram = (trainingProgramId) => {
  return fetch(`${dayApiUrl}/byTrainingProgram/${trainingProgramId}`)
    .then((res) => res.json())
    .catch((error) => console.error("Error on this", error));
};

export const getDayById = (id) => {
  return fetch(`${dayApiUrl}/${id}`)
    .then((res) => res.json())
    .catch((error) => console.error(`no day ${id}:`, error));
};

export const addDay = (day) => {
  return fetch(dayApiUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(day),
  })
    .then((res) => res.json())
    .catch((error) => console.error("did not add the day:", error));
};

export const updateDay = (day) => {
  return fetch(`${dayApiUrl}/${day.id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(day),
  })
    .then((res) => res.json())
    .catch((error) => console.error("not updating day huh", error));
};

export const deleteDay = (id) => {
  return fetch(`${dayApiUrl}/${id}`, {
    method: "DELETE",
  })
    .then((res) => res.json())
    .catch((error) => console.error("no delete day:", error));
};
