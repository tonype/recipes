export function getDifficultyLabel(difficulty: number): string {
  if (difficulty <= 2) {
    return 'Easy';
  } else if (difficulty <= 4) {
    return 'Medium';
  } else {
    return 'Hard';
  }
}

export function formatTime(minutes: number): string {
  if (minutes < 60) {
    return `${minutes} min`;
  }

  const hours = Math.floor(minutes / 60);
  const remainingMinutes = minutes % 60;

  if (remainingMinutes === 0) {
    return `${hours} hr`;
  }

  return `${hours} hr ${remainingMinutes} min`;
}
