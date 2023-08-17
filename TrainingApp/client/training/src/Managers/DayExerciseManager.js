const dayExerciseApiUrl = "https://localhost:5001/api/dayexercise";

export const addExerciseToDay = (dayExercise) => {
  return fetch(dayExerciseApiUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(dayExercise),
  })
    .then((res) => res.json())
    .catch((error) => console.error("no add", error));
};
export const getExercisesForDay = async (dayId) => {
  const response = await fetch(
    `https://localhost:5001/api/DayExercise/day/${dayId}`
  );

  if (!response.ok) {
    console.error("Failedto fetch yo", dayId);
    return [];
  }

  return response.json();
};

export const getDayIdByDayNumber = async (dayNumber) => {
  const response = await fetch(
    `${dayExerciseApiUrl}/getDayIdByDayNumber/${dayNumber}`
  );
  if (!response.ok) {
    throw new Error("didnt fetch day id");
  }
  const dayId = await response.json();
  return dayId;
};

export const fetchExercisesForUserDay = async (userId, dayNumber) => {
  const response = await fetch(
    `${dayExerciseApiUrl}/getExercisesForUserDay/${userId}/${dayNumber}`
  );
  if (!response.ok) {
    throw new Error("Failed to fetch exercises");
  }
  const exercises = await response.json();
  return exercises;
};
