const apiUrl = "https://localhost:5001/api/SetLog";

export const getAllSetLogs = () => {
  return fetch(apiUrl).then((response) => response.json());
};

export const getSetLogById = (id) => {
  return fetch(`${apiUrl}/${id}`).then((response) => response.json());
};

export const addSetLog = (setLog) => {
  return fetch(apiUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(setLog),
  }).then((response) => response.json());
};

export const updateSetLog = (setLog) => {
  return fetch(`${apiUrl}/${setLog.Id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(setLog),
  });
};

export const deleteSetLog = (id) => {
  return fetch(`${apiUrl}/${id}`, {
    method: "DELETE",
  });
};

export const getSetLogsByUserAndDate = (userId, formattedDate) => {
  return fetch(`${apiUrl}/user/${userId}/date/${formattedDate}`).then(
    (response) => {
      if (!response.ok) {
        throw new Error("do better");
      }
      return response.json();
    }
  );
};
